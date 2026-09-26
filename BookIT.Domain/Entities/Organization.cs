namespace BookIT.Domain.Entities;

public class Organization
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public ICollection<Staff> Staff { get; set; } = [];
    public ICollection<Customer> Customers { get; set; } = [];
    public ICollection<Service> Services { get; set; } = [];
    public ICollection<WorkingHour> WorkingHours { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
}