namespace BookIT.Domain.Entities;

public class Customer
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public long OrganizationId { get; set; }

    public Organization Organization { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = [];
}