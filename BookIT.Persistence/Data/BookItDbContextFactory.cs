using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookIT.Persistence.Data;

public class BookItDbContextFactory : IDesignTimeDbContextFactory<BookItDbContext>
{
    public BookItDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("BOOKIT_POSTGRES_CONNECTION")
            ?? "host=localhost;port=5432;database=bookit_nepal;username=postgres;password=postgres;Pooling=true;";

        var options = new DbContextOptionsBuilder<BookItDbContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new BookItDbContext(options);
    }
}