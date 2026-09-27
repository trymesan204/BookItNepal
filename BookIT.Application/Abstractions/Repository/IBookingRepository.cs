using BookIT.Domain.Entities;

namespace BookIT.Application.Abstractions.Repository;

public interface IBookingRepository
{
    Task<List<Booking>> GetByOrganizationIdAsync(
        long organizationId,
        CancellationToken cancellationToken);

    Task<Booking?> GetByOrganizationIdAndBookingIdAsync(
        long organizationId,
        long bookingId,
        CancellationToken cancellationToken);

    Task<List<Booking>> GetByStaffIdAsync(
        long organizationId,
        long staffId,
        CancellationToken cancellationToken);

    Task<List<Booking>> GetByCustomerIdAsync(
        long organizationId,
        long customerId,
        CancellationToken cancellationToken);

    Task<List<Booking>> GetByServiceIdAsync(
        long organizationId,
        long serviceId,
        CancellationToken cancellationToken);

    Task<Booking?> GetByPublicTokenAsync(
        Guid publicToken,
        CancellationToken cancellationToken);

    Task AddAsync(Booking booking, CancellationToken cancellationToken);
    void Update(Booking booking);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}