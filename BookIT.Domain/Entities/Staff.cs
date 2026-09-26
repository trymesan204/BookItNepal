using BookIT.Domain.Enums;

namespace BookIT.Domain.Entities;

public class Staff
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PasswordHash { get; set; }
    public StaffType StaffType { get; set; }
    public long OrganizationId { get; set; }

    public Organization Organization { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = [];
}