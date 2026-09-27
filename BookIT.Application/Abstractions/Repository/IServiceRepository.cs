using BookIT.Domain.Entities;

namespace BookIT.Application.Abstractions.Repository;

public interface IServiceRepository
{
    Task<List<Service>> GetByOrganizationIdAsync(
        long organizationId,
        CancellationToken cancellationToken);

    Task<Service?> GetByOrganizationIdAndServiceIdAsync(
        long organizationId,
        long serviceId,
        CancellationToken cancellationToken);

    Task<List<Service>> GetPublicByOrganizationIdAsync(
        long organizationId,
        CancellationToken cancellationToken);

    Task AddAsync(Service service, CancellationToken cancellationToken);
    void Update(Service service);
    void Remove(Service service);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}