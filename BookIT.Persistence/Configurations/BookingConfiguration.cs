using BookIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIT.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(booking => booking.Id);
        builder.Property(booking => booking.Id).UseIdentityByDefaultColumn();
        builder.Property(booking => booking.BookingDate).IsRequired();
        builder.Property(booking => booking.StartTimeMinutes).IsRequired();
        builder.Property(booking => booking.EndTimeMinutes).IsRequired();
        builder.Property(booking => booking.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(booking => booking.PublicToken).IsRequired();
        builder.HasIndex(booking => booking.PublicToken).IsUnique();
        builder.HasIndex(booking => new { booking.StaffId, booking.BookingDate });

        builder.HasOne(booking => booking.Organization)
            .WithMany(organization => organization.Bookings)
            .HasForeignKey(booking => booking.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(booking => booking.Customer)
            .WithMany(customer => customer.Bookings)
            .HasForeignKey(booking => booking.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(booking => booking.Service)
            .WithMany(service => service.Bookings)
            .HasForeignKey(booking => booking.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(booking => booking.Staff)
            .WithMany(staff => staff.Bookings)
            .HasForeignKey(booking => booking.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}