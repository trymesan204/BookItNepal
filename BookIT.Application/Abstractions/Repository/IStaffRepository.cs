using BookIT.Domain.Entities;

namespace BookIT.Application.Abstractions.Repository;

public interface IStaffRepository
{
    Task<Staff?> GetByOrganizationAndPhoneNumberAsync(
        string phoneNumber,
        long organizationId,
        CancellationToken cancellationToken);

    Task<List<Staff>> GetByOrganizationIdAsync(
        long organizationId,
        CancellationToken cancellationToken);

    Task<Staff?> GetByOrganizationIdAndStaffIdAsync(
        long organizationId,
        long staffId,
        CancellationToken cancellationToken);

    Task AddAsync(Staff staff, CancellationToken cancellationToken);
    void Update(Staff staff);
    void Remove(Staff staff);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}