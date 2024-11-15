using Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CustomerManagement>();
var app = builder.Build();

app.MapGet("/", async (CustomerManagement customers) =>
{
    await customers.CreateCustomer("John Doe");
    var allCustomers = await customers.GetCustomers();
    return Results.Ok(allCustomers);
});

app.Run();
