using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItNepal.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Manager")]
[Route("api/staff")]
public sealed class StaffController(IStaffService staffService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStaff(CancellationToken cancellationToken)
    {
        try
        {
            var staff = await staffService.GetStaffByOrganizationAsync(cancellationToken);
            return Ok(staff);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve staff.");
        }
    }

    [HttpGet("{staffId:long}")]
    public async Task<IActionResult> GetByStaffId(long staffId, CancellationToken cancellationToken)
    {
        try
        {
            var staff = await staffService.GetStaffByOrganizationAndStaffIdAsync(staffId, cancellationToken);
            return staff is null ? NotFound() : Ok(staff);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve the staff member.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(StaffDTO staffDto, CancellationToken cancellationToken)
    {
        try
        {
            var createdStaff = await staffService.CreateStaffAsync(staffDto, cancellationToken);
            return Ok(createdStaff);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to create the staff member.");
        }
    }

    [HttpPut("{staffId:long}")]
    public async Task<IActionResult> Put(long staffId, StaffDTO staffDto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await staffService.UpdateStaffAsync(staffId, staffDto, cancellationToken);
            return updated ? NoContent() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to update the staff member.");
        }
    }

    [HttpDelete("{staffId:long}")]
    public async Task<IActionResult> Delete(long staffId, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await staffService.DeleteStaffByOrganizationAsync(staffId, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to delete the staff member.");
        }
    }
}
