using BookIT.Application.Abstractions.Repository;
using BookIT.Domain.Entities;
using BookIT.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookIT.Persistence.Repositories;

public sealed class BookingRepository(BookItDbContext dbContext) : IBookingRepository
{
    public Task<List<Booking>> GetByOrganizationIdAsync(
        long organizationId,
        CancellationToken cancellationToken)
    {
        return dbContext.Bookings
            .AsNoTracking()
            .Where(booking => booking.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public Task<Booking?> GetByOrganizationIdAndBookingIdAsync(
        long organizationId,
        long bookingId,
        CancellationToken cancellationToken)
    {
        return dbContext.Bookings.SingleOrDefaultAsync(
            booking => booking.OrganizationId == organizationId && booking.Id == bookingId,
            cancellationToken);
    }

    public Task<List<Booking>> GetByStaffIdAsync(
        long organizationId,
        long staffId,
        CancellationToken cancellationToken)
    {
        return dbContext.Bookings
            .AsNoTracking()
            .Where(booking => booking.OrganizationId == organizationId && booking.StaffId == staffId)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Booking>> GetByCustomerIdAsync(
        long organizationId,
        long customerId,
        CancellationToken cancellationToken)
    {
        return dbContext.Bookings
            .AsNoTracking()
            .Where(booking => booking.OrganizationId == organizationId && booking.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Booking>> GetByServiceIdAsync(
        long organizationId,
        long serviceId,
        CancellationToken cancellationToken)
    {
        return dbContext.Bookings
            .AsNoTracking()
            .Where(booking => booking.OrganizationId == organizationId && booking.ServiceId == serviceId)
            .ToListAsync(cancellationToken);
    }

    public Task<Booking?> GetByPublicTokenAsync(Guid publicToken, CancellationToken cancellationToken)
    {
        return dbContext.Bookings.SingleOrDefaultAsync(
            booking => booking.PublicToken == publicToken,
            cancellationToken);
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken)
    {
        return dbContext.Bookings.AddAsync(booking, cancellationToken).AsTask();
    }

    public void Update(Booking booking)
    {
        dbContext.Bookings.Update(booking);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
