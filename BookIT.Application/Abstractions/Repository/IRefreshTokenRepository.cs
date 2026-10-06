using BookIT.Domain.Entities;

namespace BookIT.Application.Abstractions.Repository;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<bool> TryRevokeAsync(long id, DateTimeOffset revokedAt, CancellationToken cancellationToken);
    Task<bool> TryRotateAsync(
        long currentId,
        RefreshToken replacement,
        DateTimeOffset revokedAt,
        CancellationToken cancellationToken);
    Task RevokeAllActiveForStaffAsync(long staffId, DateTimeOffset revokedAt, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
