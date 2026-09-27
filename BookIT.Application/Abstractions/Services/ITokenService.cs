using BookIT.Domain.Enums;

namespace BookIT.Application.Abstractions.Services;
public interface ITokenService
{
    string CreateToken(long userId, long organizationId, StaffType staffType);
}

