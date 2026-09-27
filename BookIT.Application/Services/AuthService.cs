using BookIT.Application.Abstractions.Context;
using BookIT.Application.Abstractions.Repository;
using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Auth;

namespace BookIT.Application.Services;

public sealed class AuthService(
    IStaffRepository staffRepository,
    IOrganizationContext organizationContext,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<LoginResponseDTO?> LoginByPhoneNumberAsync(
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

        return new LoginResponseDTO(
            token,
            staff.Id,
            staff.OrganizationId,
            staff.StaffType.ToString());
    }
}
