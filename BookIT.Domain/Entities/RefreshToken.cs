namespace BookIT.Domain.Entities;

public class RefreshToken
{
    public long Id { get; set; }
    public long StaffId { get; set; }
    public required string TokenHash { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }

    public Staff Staff { get; set; } = null!;
}
