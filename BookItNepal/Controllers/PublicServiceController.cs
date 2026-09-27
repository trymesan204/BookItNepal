using BookIT.Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItNepal.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/{orgSlug}/services")]
public sealed class PublicServiceController(IServiceManagementService serviceManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var services = await serviceManagementService.GetPublicServicesAsync(cancellationToken);
            return Ok(services);
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound();
        }
        catch
        {
            return Problem("Unable to retrieve public services.");
        }
    }
}
