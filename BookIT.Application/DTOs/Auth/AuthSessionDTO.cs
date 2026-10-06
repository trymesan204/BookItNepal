namespace BookIT.Application.DTOs.Auth;

public sealed record AuthSessionDTO(
    LoginResponseDTO Response,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);
