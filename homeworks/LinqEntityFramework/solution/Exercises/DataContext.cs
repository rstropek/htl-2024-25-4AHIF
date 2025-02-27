using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Exercises;

public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<OrderHeader> Orders => Set<OrderHeader>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderLine>()
            .Property(ol => ol.UnitPrice)
            .HasConversion<double>();
    }

    public async Task CleanAndFillWithSampleData()
    {
        using var transaction = await Database.BeginTransactionAsync();
        try
        {
            await OrderLines.ExecuteDeleteAsync();
            await Orders.ExecuteDeleteAsync();
            await Customers.ExecuteDeleteAsync();
            
            await Customers.AddRangeAsync(SampleData.Customers);
            await SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
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