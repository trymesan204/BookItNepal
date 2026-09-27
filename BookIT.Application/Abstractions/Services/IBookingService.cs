using BookIT.Application.DTOs.Booking;

namespace BookIT.Application.Abstractions.Services;

public interface IBookingService
{
    Task<List<BookingDTO>> GetBookingsByOrganizationAsync(
        CancellationToken cancellationToken);

    Task<BookingDTO?> GetBookingByOrganizationAndBookingIdAsync(
        long bookingId,
        CancellationToken cancellationToken);

    Task<List<BookingDTO>> GetBookingsByStaffAsync(
        long staffId,
        CancellationToken cancellationToken);

    Task<List<BookingDTO>> GetBookingsByCustomerAsync(
        long customerId,
        CancellationToken cancellationToken);

    Task<List<BookingDTO>> GetBookingsByServiceAsync(
        long serviceId,
        CancellationToken cancellationToken);

    Task<BookingDTO> CreateBookingAsync(
        BookingDTO bookingDto,
        CancellationToken cancellationToken);

    Task<BookingDTO?> GetBookingByPublicTokenAsync(
        Guid publicToken,
        CancellationToken cancellationToken);

    Task<bool> UpdateBookingAsync(
        long bookingId,
        BookingDTO bookingDto,
        CancellationToken cancellationToken);

    Task<bool> CancelPublicBookingAsync(
        Guid publicToken,
        CancellationToken cancellationToken);
}
