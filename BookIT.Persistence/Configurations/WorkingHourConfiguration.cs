using BookIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIT.Persistence.Configurations;

public class WorkingHourConfiguration : IEntityTypeConfiguration<WorkingHour>
{
    public void Configure(EntityTypeBuilder<WorkingHour> builder)
    {
        builder.HasKey(workingHour => workingHour.Id);
        builder.Property(workingHour => workingHour.Id).UseIdentityByDefaultColumn();
        builder.Property(workingHour => workingHour.OpeningHourMinutes).IsRequired();
        builder.Property(workingHour => workingHour.ClosingHourMinutes).IsRequired();
        builder.Property(workingHour => workingHour.DayOfWeek).IsRequired();
        builder.HasOne(workingHour => workingHour.Organization)
            .WithMany(organization => organization.WorkingHours)
            .HasForeignKey(workingHour => workingHour.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}