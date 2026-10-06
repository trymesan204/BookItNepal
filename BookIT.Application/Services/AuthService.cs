using BookIT.Application.Abstractions.Context;
using BookIT.Application.Abstractions.Repository;
using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Auth;
using BookIT.Domain.Entities;

namespace BookIT.Application.Services;

public sealed class AuthService(
    IStaffRepository staffRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IOrganizationContext organizationContext,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IAuthService
{
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromHours(8);

    public async Task<AuthSessionDTO?> LoginByPhoneNumberAsync(
        LoginDTO loginDto,
        CancellationToken cancellationToken)
    {
        var organizationId = organizationContext.OrganizationId
            ?? throw new UnauthorizedAccessException("Organization was not resolved.");
        var staff = await staffRepository.GetByOrganizationAndPhoneNumberAsync(
            loginDto.PhoneNumber,
            organizationId,
            cancellationToken);

        if (staff is null || !passwordHasher.Verify(loginDto.Password, staff.PasswordHash))
        {
            return null;
        }

        var token = tokenService.CreateToken(
            staff.Id,
            staff.OrganizationId,
            staff.StaffType);

        return await CreateSessionAsync(staff, token, cancellationToken);
    }

    public async Task<AuthSessionDTO?> RefreshAsync(
        string? presentedRefreshToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(presentedRefreshToken))
        {
            return null;
        }

        var storedToken = await refreshTokenRepository.GetByTokenHashAsync(
            tokenService.HashRefreshToken(presentedRefreshToken),
            cancellationToken);
        if (storedToken is null)
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        if (storedToken.RevokedAt is not null)
        {
            await refreshTokenRepository.RevokeAllActiveForStaffAsync(
                storedToken.StaffId,
                now,
                cancellationToken);
            return null;
        }

        if (storedToken.ExpiresAt <= now)
        {
            return null;
        }

        if (organizationContext.OrganizationId != storedToken.Staff.OrganizationId)
        {
            return null;
        }

        var staff = storedToken.Staff;
        var accessToken = tokenService.CreateToken(staff.Id, staff.OrganizationId, staff.StaffType);
        var refreshToken = tokenService.CreateRefreshToken();
        var expiresAt = now.Add(RefreshTokenLifetime);
        var rotated = await refreshTokenRepository.TryRotateAsync(
            storedToken.Id,
            new RefreshToken
            {
                StaffId = staff.Id,
                TokenHash = tokenService.HashRefreshToken(refreshToken),
                ExpiresAt = expiresAt
            },
            now,
            cancellationToken);

        if (!rotated)
        {
            await refreshTokenRepository.RevokeAllActiveForStaffAsync(
                storedToken.StaffId,
                now,
                cancellationToken);
            return null;
        }

        return new AuthSessionDTO(
            new LoginResponseDTO(accessToken, staff.Id, staff.OrganizationId, staff.StaffType.ToString()),
            refreshToken,
            expiresAt);
    }

    public async Task LogoutAsync(string? refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        var storedToken = await refreshTokenRepository.GetByTokenHashAsync(
            tokenService.HashRefreshToken(refreshToken),
            cancellationToken);
        if (storedToken is not null)
        {
            await refreshTokenRepository.TryRevokeAsync(
                storedToken.Id,
                DateTimeOffset.UtcNow,
                cancellationToken);
        }
    }

    private async Task<AuthSessionDTO> CreateSessionAsync(
        Staff staff,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var refreshToken = tokenService.CreateRefreshToken();
        var expiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifetime);
        await refreshTokenRepository.AddAsync(
            new RefreshToken
            {
                StaffId = staff.Id,
                TokenHash = tokenService.HashRefreshToken(refreshToken),
                ExpiresAt = expiresAt
            },
            cancellationToken);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return new AuthSessionDTO(
            new LoginResponseDTO(accessToken, staff.Id, staff.OrganizationId, staff.StaffType.ToString()),
            refreshToken,
            expiresAt);
    }
}
