using BookIT.Application.Abstractions.Context;

namespace BookItNepal.Infrastructure;

public sealed class OrganizationContext : IOrganizationContext
{
    public long? OrganizationId { get; set; }
}
