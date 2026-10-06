using BookIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookIT.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_token");
        builder.HasKey(refreshToken => refreshToken.Id);
        builder.Property(refreshToken => refreshToken.Id).UseIdentityByDefaultColumn();
        builder.Property(refreshToken => refreshToken.TokenHash).HasMaxLength(64).IsRequired();
        builder.Property(refreshToken => refreshToken.ExpiresAt).IsRequired();
        builder.HasIndex(refreshToken => refreshToken.TokenHash).IsUnique();
        builder.HasIndex(refreshToken => refreshToken.StaffId);
        builder.HasOne(refreshToken => refreshToken.Staff)
            .WithMany(staff => staff.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.StaffId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
