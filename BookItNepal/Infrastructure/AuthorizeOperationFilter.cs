using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace BookItNepal.Infrastructure;

public sealed class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.MethodInfo;
        var controller = method.DeclaringType;
        var hasAuthorize = method.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any()
            || controller?.GetCustomAttributes<AuthorizeAttribute>(inherit: true).Any() == true;
        var hasAllowAnonymous = method.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true).Any()
            || controller?.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true).Any() == true;

        if (!hasAuthorize || hasAllowAnonymous)
        {
            return;
        }

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}