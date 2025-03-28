using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Tennis.DataAccess;

public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Point> Points => Set<Point>();
}

public class ApplicationDataContextFactory : IDesignTimeDbContextFactory<ApplicationDataContext>
{
    public ApplicationDataContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDataContext>();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

        return new ApplicationDataContext(optionsBuilder.Options);
    }
}