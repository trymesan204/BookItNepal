using BookIT.Domain.Enums;

namespace BookIT.Domain.Entities;

public class Booking
{
    public long Id { get; set; }
    public long OrganizationId { get; set; }
    public long CustomerId { get; set; }
    public long ServiceId { get; set; }
    public long StaffId { get; set; }
    public DateOnly BookingDate { get; set; }
    public int StartTimeMinutes { get; set; }
    public int EndTimeMinutes { get; set; }
    public BookingStatus Status { get; set; }
    public Guid PublicToken { get; set; }

    public Organization Organization { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public Staff Staff { get; set; } = null!;
}