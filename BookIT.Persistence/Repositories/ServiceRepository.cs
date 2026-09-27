using BookIT.Application.Abstractions.Repository;
using BookIT.Domain.Entities;
using BookIT.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookIT.Persistence.Repositories;

public sealed class ServiceRepository(BookItDbContext dbContext) : IServiceRepository
{
    public Task<List<Service>> GetByOrganizationIdAsync(
        long organizationId,
        CancellationToken cancellationToken)
    {
        return dbContext.Services
            .AsNoTracking()
            .Where(service => service.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public Task<Service?> GetByOrganizationIdAndServiceIdAsync(
        long organizationId,
        long serviceId,
        CancellationToken cancellationToken)
    {
        return dbContext.Services.SingleOrDefaultAsync(
            service => service.OrganizationId == organizationId && service.Id == serviceId,
            cancellationToken);
    }

    public Task<List<Service>> GetPublicByOrganizationIdAsync(
        long organizationId,
        CancellationToken cancellationToken)
    {
        return dbContext.Services
            .AsNoTracking()
            .Where(service => service.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(Service service, CancellationToken cancellationToken)
    {
        return dbContext.Services.AddAsync(service, cancellationToken).AsTask();
    }

    public void Update(Service service)
    {
        dbContext.Services.Update(service);
    }

    public void Remove(Service service)
    {
        dbContext.Services.Remove(service);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
