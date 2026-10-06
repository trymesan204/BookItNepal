using BookIT.Application.Abstractions.Repository;
using BookIT.Domain.Entities;
using BookIT.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookIT.Persistence.Repositories;

public sealed class RefreshTokenRepository(BookItDbContext dbContext) : IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken)
    {
        return dbContext.Set<RefreshToken>()
            .Include(refreshToken => refreshToken.Staff)
            .SingleOrDefaultAsync(
                refreshToken => refreshToken.TokenHash == tokenHash,
                cancellationToken);
    }

    public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        return dbContext.Set<RefreshToken>().AddAsync(refreshToken, cancellationToken).AsTask();
    }

    public async Task<bool> TryRevokeAsync(
        long id,
        DateTimeOffset revokedAt,
        CancellationToken cancellationToken)
    {
        return await dbContext.Set<RefreshToken>()
            .Where(refreshToken => refreshToken.Id == id && refreshToken.RevokedAt == null)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(refreshToken => refreshToken.RevokedAt, revokedAt),
                cancellationToken) == 1;
    }

    public async Task<bool> TryRotateAsync(
        long currentId,
        RefreshToken replacement,
        DateTimeOffset revokedAt,
        CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var revoked = await dbContext.Set<RefreshToken>()
            .Where(refreshToken => refreshToken.Id == currentId && refreshToken.RevokedAt == null)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(refreshToken => refreshToken.RevokedAt, revokedAt),
                cancellationToken);

        if (revoked != 1)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        await dbContext.Set<RefreshToken>().AddAsync(replacement, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return true;
    }

    public async Task RevokeAllActiveForStaffAsync(
        long staffId,
        DateTimeOffset revokedAt,
        CancellationToken cancellationToken)
    {
        await dbContext.Set<RefreshToken>()
            .Where(refreshToken => refreshToken.StaffId == staffId && refreshToken.RevokedAt == null)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(refreshToken => refreshToken.RevokedAt, revokedAt),
                cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
