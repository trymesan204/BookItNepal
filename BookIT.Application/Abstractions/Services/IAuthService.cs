using BookIT.Application.DTOs.Auth;

namespace BookIT.Application.Abstractions.Services;

public interface IAuthService
{
    Task<LoginResponseDTO?> LoginByPhoneNumberAsync(LoginDTO loginDto, CancellationToken cancellationToken);
}

