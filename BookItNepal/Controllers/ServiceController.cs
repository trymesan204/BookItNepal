using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItNepal.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Manager")]
[Route("api/services")]
public sealed class ServiceController(IServiceManagementService serviceManagementService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var services = await serviceManagementService.GetServicesByOrganizationAsync(cancellationToken);
            return Ok(services);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve services.");
        }
    }

    [HttpGet("{serviceId:long}")]
    public async Task<IActionResult> GetByServiceId(long serviceId, CancellationToken cancellationToken)
    {
        try
        {
            var service = await serviceManagementService.GetServiceByOrganizationAndServiceIdAsync(serviceId, cancellationToken);
            return service is null ? NotFound() : Ok(service);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve the service.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(ServiceDTO serviceDto, CancellationToken cancellationToken)
    {
        try
        {
            var createdService = await serviceManagementService.CreateServiceAsync(serviceDto, cancellationToken);
            return Ok(createdService);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to create the service.");
        }
    }

    [HttpPut("{serviceId:long}")]
    public async Task<IActionResult> Put(long serviceId, ServiceDTO serviceDto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await serviceManagementService.UpdateServiceAsync(serviceId, serviceDto, cancellationToken);
            return updated ? NoContent() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to update the service.");
        }
    }

    [HttpDelete("{serviceId:long}")]
    public async Task<IActionResult> Delete(long serviceId, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await serviceManagementService.DeleteServiceByOrganizationAsync(serviceId, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to delete the service.");
        }
    }
}
