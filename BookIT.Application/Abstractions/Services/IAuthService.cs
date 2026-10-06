using BookIT.Application.DTOs.Auth;

namespace BookIT.Application.Abstractions.Services;

public interface IAuthService
{
    Task<AuthSessionDTO?> LoginByPhoneNumberAsync(LoginDTO loginDto, CancellationToken cancellationToken);
    Task<AuthSessionDTO?> RefreshAsync(string? refreshToken, CancellationToken cancellationToken);
    Task LogoutAsync(string? refreshToken, CancellationToken cancellationToken);
}
