using BookIT.Application.Abstractions.Repository;
using BookIT.Domain.Entities;
using BookIT.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookIT.Persistence.Repositories;

public sealed class StaffRepository(BookItDbContext dbContext) : IStaffRepository
{
    public Task<Staff?> GetByOrganizationAndPhoneNumberAsync(
        string phoneNumber,
        long organizationId,
        CancellationToken cancellationToken)
    {
        return dbContext.Staff.SingleOrDefaultAsync(
            staff => staff.PhoneNumber == phoneNumber && staff.OrganizationId == organizationId,
            cancellationToken);
    }

    public Task<List<Staff>> GetByOrganizationIdAsync(
        long organizationId,
        CancellationToken cancellationToken)
    {
        return dbContext.Staff
            .AsNoTracking()
            .Where(staff => staff.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public Task<Staff?> GetByOrganizationIdAndStaffIdAsync(
        long organizationId,
        long staffId,
        CancellationToken cancellationToken)
    {
        return dbContext.Staff.SingleOrDefaultAsync(
            staff => staff.OrganizationId == organizationId && staff.Id == staffId,
            cancellationToken);
    }

    public Task AddAsync(Staff staff, CancellationToken cancellationToken)
    {
        return dbContext.Staff.AddAsync(staff, cancellationToken).AsTask();
    }

    public void Update(Staff staff)
    {
        dbContext.Staff.Update(staff);
    }

    public void Remove(Staff staff)
    {
        dbContext.Staff.Remove(staff);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
