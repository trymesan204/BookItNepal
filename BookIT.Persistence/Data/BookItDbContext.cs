using BookIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookIT.Persistence.Data;

public class BookItDbContext(DbContextOptions<BookItDbContext> options) : DbContext(options)
{
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<WorkingHour> WorkingHours => Set<WorkingHour>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookItDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}