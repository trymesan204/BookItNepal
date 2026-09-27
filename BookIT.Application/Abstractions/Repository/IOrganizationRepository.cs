using BookIT.Domain.Entities;

namespace BookIT.Application.Abstractions.Repository;

public interface IOrganizationRepository
{
    Task<Organization?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
}