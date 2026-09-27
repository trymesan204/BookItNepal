using System.Security.Claims;
using BookIT.Application.Abstractions.Repository;
using BookIT.Application.Abstractions.Context;

namespace BookItNepal.Infrastructure;

public sealed class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        IOrganizationContext organizationContext,
        IOrganizationRepository organizationRepository)
    {
        var organizationClaim = httpContext.User.FindFirstValue("organizationId");
        if (long.TryParse(organizationClaim, out var organizationId))
        {
            organizationContext.OrganizationId = organizationId;
        }
        else if (httpContext.Request.RouteValues.TryGetValue("orgSlug", out var routeValue)
            && routeValue is string organizationSlug)
        {
            var organization = await organizationRepository.GetBySlugAsync(
                organizationSlug,
                httpContext.RequestAborted);
            if (organization is not null)
            {
                organizationContext.OrganizationId = organization.Id;
            }
        }

        await next(httpContext);
    }
}
