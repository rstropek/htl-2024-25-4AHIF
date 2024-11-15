using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: NonParallelizable]

namespace Data.Tests;

[TestFixture]
public class DbIntegrationTests
{
    private ServiceProvider? ServiceProvider = null;

    private static ServiceProvider BuildServiceProvider()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
       
        services.AddDbContext<ApplicationDataContext>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<CustomerManagement>();

        return services.BuildServiceProvider();
    }

    [OneTimeSetUp]
    public void Setup()
    {
        ServiceProvider = BuildServiceProvider();

        using var scope = ServiceProvider!.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
        context.Database.EnsureCreatedAsync().Wait();
        context.Customers.ExecuteDeleteAsync().Wait();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ServiceProvider?.Dispose();
    }

    [Test]
    public async Task Test1()
    {
        using var scope = ServiceProvider!.CreateScope();
        var customers = scope.ServiceProvider.GetRequiredService<CustomerManagement>();

        await customers.CreateCustomer("John Doe");
        var allCustomers = await customers.GetCustomers();

        Assert.That(allCustomers, Has.Count.EqualTo(1));
    }
}