using BookIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIT.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(customer => customer.Id);
        builder.Property(customer => customer.Id).UseIdentityByDefaultColumn();
        builder.Property(customer => customer.Name).HasMaxLength(200).IsRequired();
        builder.Property(customer => customer.PhoneNumber).HasMaxLength(32).IsRequired();
        builder.Property(customer => customer.Email).HasMaxLength(320);
        builder.HasIndex(customer => new { customer.PhoneNumber, customer.OrganizationId }).IsUnique();
        builder.HasOne(customer => customer.Organization)
            .WithMany(organization => organization.Customers)
            .HasForeignKey(customer => customer.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}