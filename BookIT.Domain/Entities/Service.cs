namespace BookIT.Domain.Entities;

public class Service
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int DurationInMinutes { get; set; }
    public long OrganizationId { get; set; }

    public Organization Organization { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = [];
}