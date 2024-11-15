using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Data;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
}

public class CustomerManagement(ApplicationDataContext context)
{
    public async Task<Customer> CreateCustomer(string name)
    {
        var customer = new Customer { Name = name };
        context.Customers.Add(customer);
        await context.SaveChangesAsync();
        return customer;
    }

    public async Task<List<Customer>> GetCustomers()
    {
        return await context.Customers.ToListAsync();
    }
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDataContext>
{
    public ApplicationDataContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDataContext>();
        optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));

        return new ApplicationDataContext(optionsBuilder.Options);
    }
}