using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItNepal.Controllers;

[ApiController]
[Route("api/{orgSlug}/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginByPhoneNumber(LoginDTO loginDto, CancellationToken cancellationToken)
    {
        try
        {
            var response = await authService.LoginByPhoneNumberAsync(loginDto, cancellationToken);
            return response is null ? Unauthorized() : Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to authenticate the staff member.");
        }
    }
}
