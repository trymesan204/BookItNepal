using BookIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIT.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(organization => organization.Id);
        builder.Property(organization => organization.Id).UseIdentityByDefaultColumn();
        builder.Property(organization => organization.Name).HasMaxLength(200).IsRequired();
        builder.Property(organization => organization.Slug).HasMaxLength(200).IsRequired();
        builder.Property(organization => organization.Address).HasMaxLength(500);
        builder.Property(organization => organization.Email).HasMaxLength(320);
        builder.Property(organization => organization.PhoneNumber).HasMaxLength(32);
        builder.HasIndex(organization => organization.Slug).IsUnique();
    }
}