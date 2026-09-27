using BookIT.Application.Abstractions.Repository;
using BookIT.Domain.Entities;
using BookIT.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookIT.Persistence.Repositories;

public sealed class OrganizationRepository(BookItDbContext dbContext) : IOrganizationRepository
{
    public Task<Organization?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return dbContext.Organizations.SingleOrDefaultAsync(
            organization => organization.Slug == slug,
            cancellationToken);
    }
}
