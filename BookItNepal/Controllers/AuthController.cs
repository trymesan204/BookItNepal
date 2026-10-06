using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItNepal.Controllers;

[ApiController]
[Route("api/{orgSlug}/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    private const string RefreshTokenCookieName = "refreshToken";
    private static readonly TimeSpan RefreshTokenCookieLifetime = TimeSpan.FromHours(8);

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType<LoginResponseDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginByPhoneNumber(LoginDTO loginDto, CancellationToken cancellationToken)
    {
        AuthSessionDTO? session;
        try
        {
            session = await authService.LoginByPhoneNumberAsync(loginDto, cancellationToken);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }

        if (session is null)
        {
            return Unauthorized();
        }

        SetRefreshTokenCookie(session.RefreshToken, session.RefreshTokenExpiresAt);
        return Ok(session.Response);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType<LoginResponseDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        var session = await authService.RefreshAsync(refreshToken, cancellationToken);
        if (session is null)
        {
            ClearRefreshTokenCookie();
            return Unauthorized();
        }

        SetRefreshTokenCookie(session.RefreshToken, session.RefreshTokenExpiresAt);
        return Ok(session.Response);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await authService.LogoutAsync(Request.Cookies[RefreshTokenCookieName], cancellationToken);
        ClearRefreshTokenCookie();
        return NoContent();
    }

    private void SetRefreshTokenCookie(string refreshToken, DateTimeOffset expiresAt)
    {
        Response.Cookies.Append(
            RefreshTokenCookieName,
            refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = expiresAt,
                MaxAge = RefreshTokenCookieLifetime
            });
    }

    private void ClearRefreshTokenCookie()
    {
        Response.Cookies.Delete(
            RefreshTokenCookieName,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });
    }
}
