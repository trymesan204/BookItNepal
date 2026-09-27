using BookIT.Application.Abstractions.Context;
using BookIT.Application.Abstractions.Repository;
using BookIT.Application.Abstractions.Services;
using BookIT.Application.DTOs.Booking;
using BookIT.Domain.Entities;
using BookIT.Domain.Enums;

namespace BookIT.Application.Services;

public sealed class BookingService(
    IBookingRepository bookingRepository,
    IOrganizationContext organizationContext) : IBookingService
{
    public async Task<List<BookingDTO>> GetBookingsByOrganizationAsync(CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var bookings = await bookingRepository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        return bookings.Select(ToDTO).ToList();
    }

    public async Task<BookingDTO?> GetBookingByOrganizationAndBookingIdAsync(
        long bookingId,
        CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var booking = await bookingRepository.GetByOrganizationIdAndBookingIdAsync(
            organizationId,
            bookingId,
            cancellationToken);

        return booking is null ? null : ToDTO(booking);
    }

    public async Task<List<BookingDTO>> GetBookingsByStaffAsync(
        long staffId,
        CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var bookings = await bookingRepository.GetByStaffIdAsync(
            organizationId,
            staffId,
            cancellationToken);

        return bookings.Select(ToDTO).ToList();
    }

    public async Task<List<BookingDTO>> GetBookingsByCustomerAsync(
        long customerId,
        CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var bookings = await bookingRepository.GetByCustomerIdAsync(
            organizationId,
            customerId,
            cancellationToken);

        return bookings.Select(ToDTO).ToList();
    }

    public async Task<List<BookingDTO>> GetBookingsByServiceAsync(
        long serviceId,
        CancellationToken cancellationToken)
    {
        var organizationId = GetOrganizationId();
        var bookings = await bookingRepository.GetByServiceIdAsync(
            organizationId,
            serviceId,
            cancellationToken);

        return bookings.Select(ToDTO).ToList();
    }

    public async Task<BookingDTO> CreateBookingAsync(
        BookingDTO bookingDto,
        CancellationToken cancellationToken)
    {
        var booking = new Booking
        {
            OrganizationId = GetOrganizationId(),
            CustomerId = bookingDto.CustomerId,
            ServiceId = bookingDto.ServiceId,
            StaffId = bookingDto.StaffId,
            BookingDate = bookingDto.BookingDate,
            StartTimeMinutes = bookingDto.StartTimeMinutes,
            EndTimeMinutes = bookingDto.EndTimeMinutes,
            Status = BookingStatus.Pending,
            PublicToken = bookingDto.PublicToken == Guid.Empty
                ? Guid.NewGuid()
                : bookingDto.PublicToken
        };

        await bookingRepository.AddAsync(booking, cancellationToken);
        await bookingRepository.SaveChangesAsync(cancellationToken);
        return ToDTO(booking);
    }

    public async Task<BookingDTO?> GetBookingByPublicTokenAsync(
        Guid publicToken,
        CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByPublicTokenAsync(publicToken, cancellationToken);
        if (booking is null || booking.OrganizationId != GetOrganizationId())
        {
            return null;
        }

        return ToDTO(booking);
    }

    public async Task<bool> UpdateBookingAsync(
        long bookingId,
        BookingDTO bookingDto,
        CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByOrganizationIdAndBookingIdAsync(
            GetOrganizationId(),
            bookingId,
            cancellationToken);

        if (booking is null)
        {
            return false;
        }

        booking.BookingDate = bookingDto.BookingDate;
        booking.StartTimeMinutes = bookingDto.StartTimeMinutes;
        booking.EndTimeMinutes = bookingDto.EndTimeMinutes;
        booking.Status = bookingDto.Status;

        bookingRepository.Update(booking);
        await bookingRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> CancelPublicBookingAsync(
        Guid publicToken,
        CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByPublicTokenAsync(publicToken, cancellationToken);
        if (booking is null || booking.OrganizationId != GetOrganizationId())
        {
            return false;
        }

        booking.Status = BookingStatus.Cancelled;
        bookingRepository.Update(booking);
        await bookingRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private long GetOrganizationId()
    {
        return organizationContext.OrganizationId
            ?? throw new UnauthorizedAccessException("Organization was not resolved.");
    }

    private static BookingDTO ToDTO(Booking booking)
    {
        return new BookingDTO(
            booking.Id,
            booking.CustomerId,
            booking.ServiceId,
            booking.StaffId,
            booking.BookingDate,
            booking.StartTimeMinutes,
            booking.EndTimeMinutes,
            booking.Status,
            booking.PublicToken);
    }
}
