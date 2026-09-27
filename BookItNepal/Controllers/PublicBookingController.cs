using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItNepal.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/{orgSlug}/bookings")]
public sealed class PublicBookingController(IBookingService bookingService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(BookingDTO bookingDto, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await bookingService.CreateBookingAsync(bookingDto, cancellationToken);
            return Ok(booking);
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound();
        }
        catch
        {
            return Problem("Unable to create the booking.");
        }
    }

    [HttpGet("{publicToken:guid}")]
    public async Task<IActionResult> Get(Guid publicToken, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await bookingService.GetBookingByPublicTokenAsync(publicToken, cancellationToken);
            return booking is null ? NotFound() : Ok(booking);
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound();
        }
        catch
        {
            return Problem("Unable to retrieve the booking.");
        }
    }

    [HttpPut("{publicToken:guid}/cancel")]
    public async Task<IActionResult> Put(Guid publicToken, CancellationToken cancellationToken)
    {
        try
        {
            var cancelled = await bookingService.CancelPublicBookingAsync(publicToken, cancellationToken);
            return cancelled ? NoContent() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound();
        }
        catch
        {
            return Problem("Unable to cancel the booking.");
        }
    }
}
