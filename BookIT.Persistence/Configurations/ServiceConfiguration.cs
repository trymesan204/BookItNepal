using BookIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIT.Persistence.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(service => service.Id);
        builder.Property(service => service.Id).UseIdentityByDefaultColumn();
        builder.Property(service => service.Name).HasMaxLength(200).IsRequired();
        builder.Property(service => service.Price).HasPrecision(12, 2).IsRequired();
        builder.Property(service => service.DurationInMinutes).IsRequired();
        builder.HasOne(service => service.Organization)
            .WithMany(organization => organization.Services)
            .HasForeignKey(service => service.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}