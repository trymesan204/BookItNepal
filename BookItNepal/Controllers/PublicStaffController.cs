using BookIT.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItNepal.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/{orgSlug}/staff")]
public sealed class PublicStaffController(IStaffService staffService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var staff = await staffService.GetPublicStaffAsync(cancellationToken);
            return Ok(staff);
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound();
        }
        catch
        {
            return Problem("Unable to retrieve public staff.");
        }
    }
}
