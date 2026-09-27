using BookIT.Application.Abstractions.Services;
using BookIT.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookIT.Infrastructure.Services;

public sealed class TokenService(IConfiguration configuration) : ITokenService
{
    public string CreateToken(long userId, long organizationId, StaffType staffType)
    {
        var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret is not configured.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("organizationId", organizationId.ToString()),
            new Claim(ClaimTypes.Role, staffType.ToString())
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}