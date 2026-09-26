using BookIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIT.Persistence.Configurations;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.HasKey(staff => staff.Id);
        builder.Property(staff => staff.Id).UseIdentityByDefaultColumn();
        builder.Property(staff => staff.Name).HasMaxLength(200).IsRequired();
        builder.Property(staff => staff.PhoneNumber).HasMaxLength(32).IsRequired();
        builder.Property(staff => staff.PasswordHash).HasMaxLength(512).IsRequired();
        builder.Property(staff => staff.StaffType).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.HasIndex(staff => new { staff.PhoneNumber, staff.OrganizationId }).IsUnique();
        builder.HasOne(staff => staff.Organization)
            .WithMany(organization => organization.Staff)
            .HasForeignKey(staff => staff.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}