using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookItNepal.Controllers;

[ApiController]
[Authorize]
[Route("api/bookings")]
public sealed class BookingController(IBookingService bookingService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var bookings = await bookingService.GetBookingsByOrganizationAsync(cancellationToken);
            return Ok(bookings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve bookings.");
        }
    }

    [HttpGet("{bookingId:long}")]
    public async Task<IActionResult> GetByBookingId(long bookingId, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await bookingService.GetBookingByOrganizationAndBookingIdAsync(bookingId, cancellationToken);
            return booking is null ? NotFound() : Ok(booking);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve the booking.");
        }
    }

    [HttpGet("staff/{staffId:long}")]
    public async Task<IActionResult> GetByStaffId(long staffId, CancellationToken cancellationToken)
    {
        try
        {
            var bookings = await bookingService.GetBookingsByStaffAsync(staffId, cancellationToken);
            return Ok(bookings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve bookings for the staff member.");
        }
    }

    [HttpGet("customer/{customerId:long}")]
    public async Task<IActionResult> GetByCustomerId(long customerId, CancellationToken cancellationToken)
    {
        try
        {
            var bookings = await bookingService.GetBookingsByCustomerAsync(customerId, cancellationToken);
            return Ok(bookings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve bookings for the customer.");
        }
    }

    [HttpGet("service/{serviceId:long}")]
    public async Task<IActionResult> GetByServiceId(long serviceId, CancellationToken cancellationToken)
    {
        try
        {
            var bookings = await bookingService.GetBookingsByServiceAsync(serviceId, cancellationToken);
            return Ok(bookings);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to retrieve bookings for the service.");
        }
    }

    [HttpPut("{bookingId:long}")]
    public async Task<IActionResult> Put(long bookingId, BookingDTO bookingDto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await bookingService.UpdateBookingAsync(bookingId, bookingDto, cancellationToken);
            return updated ? NoContent() : NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch
        {
            return Problem("Unable to update the booking.");
        }
    }
}
