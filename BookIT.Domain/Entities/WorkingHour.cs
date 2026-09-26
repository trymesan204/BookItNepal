namespace BookIT.Domain.Entities;

public class WorkingHour
{
    public long Id { get; set; }
    public long OrganizationId { get; set; }
    public int OpeningHourMinutes { get; set; }
    public int ClosingHourMinutes { get; set; }
    public int DayOfWeek { get; set; }

    public Organization Organization { get; set; } = null!;
}