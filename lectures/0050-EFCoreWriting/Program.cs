using System.Runtime.CompilerServices;
using Exercises;
using Microsoft.EntityFrameworkCore;

// Create the database context
var factory = new ApplicationDataContextFactory();
var context = factory.CreateDbContext(args);

// Add a record to customers
var newCustomer = new Customer(0, "Ollivanders Magic Wands", "UK", "Diagon Alley");
Console.WriteLine($"The name of the new customer is {newCustomer.CompanyName}");
Console.WriteLine($"The ID of the new customer AFTER CREATION is {newCustomer.ID}");

// Print unique ID of the new customer
Console.WriteLine($"0x{RuntimeHelpers.GetHashCode(newCustomer):X}");

// Add the new customer to the database
// Note: Adds the customer AND ALL RELATED ENTITIES
await context.Customers.AddAsync(newCustomer);
Console.WriteLine($"The ID of the new customer AFTER ADDING is {newCustomer.ID}");
await context.SaveChangesAsync();
Console.WriteLine($"The ID of the new customer AFTER SAVING is {newCustomer.ID}");

// =====================================================

// Re-read the customer from the database
var reReadCustomer = await context.Customers.FindAsync(newCustomer.ID);
Console.WriteLine($"The ID of the re-read customer is {reReadCustomer!.ID}");

// Print unique ID of the re-read customer
Console.WriteLine($"0x{RuntimeHelpers.GetHashCode(reReadCustomer):X}");

// Update the customer
reReadCustomer.CompanyName = "Ollivanders";
Console.WriteLine($"The name of the new customer is {newCustomer.CompanyName}");

// Save the changes
await context.SaveChangesAsync();

// =====================================================

// Read the customer from the database using raw SQL
using (var connection = context.Database.GetDbConnection())
{
    await connection.OpenAsync();
    using var command = connection.CreateCommand();
    command.CommandText = $"SELECT * FROM Customers WHERE ID = {reReadCustomer.ID}";

    using var reader = await command.ExecuteReaderAsync();
    await reader.ReadAsync();
    Console.WriteLine($"The name of the customer (from SQL) is {reader["CompanyName"]}");
}

// =====================================================

context.Customers.Remove(reReadCustomer);
await context.SaveChangesAsync();

if (await context.Customers.FindAsync(reReadCustomer.ID) is null)
{
    Console.WriteLine("The customer was deleted successfully");
}

// =====================================================

// Add a new customer with a related order
var newCustomerWithOrder = new Customer(0, "Malfoy's Wands", "UK", "Diagon Alley");
newCustomerWithOrder.Orders.Add(new OrderHeader(0, newCustomerWithOrder.ID, newCustomerWithOrder, new DateOnly(2024, 1, 1), "UK", "FOB", "Net 30"));

// Add the new customer and order to the database
await context.Customers.AddAsync(newCustomerWithOrder);
await context.SaveChangesAsync();

// =====================================================

// Read the customer and order from the database
var reReadCustomerWithOrder = await context.Customers
    .Include(c => c.Orders)
    .Where(c => c.ID == newCustomerWithOrder.ID)
    .FirstOrDefaultAsync();
Console.WriteLine($"The name of the customer is {reReadCustomerWithOrder!.CompanyName}");
foreach (var order in reReadCustomerWithOrder.Orders)
{
    Console.WriteLine($"The order date of the customer is {order.OrderDate}");
}

// =====================================================

var newTrackedCustomer = new Customer(0, "Gryffindor", "UK", "Diagon Alley");

// Print tracking state of the new customer
Console.WriteLine($"The tracking state of the new customer is {context.Entry(newTrackedCustomer).State}");

// Add the new customer to the database
await context.Customers.AddAsync(newTrackedCustomer);

// Print tracking state of the new customer
Console.WriteLine($"The tracking state of the new customer is {context.Entry(newTrackedCustomer).State}");

// Save the changes
await context.SaveChangesAsync();

// Print tracking state of the new customer
Console.WriteLine($"The tracking state of the new customer is {context.Entry(newTrackedCustomer).State}");

newTrackedCustomer.CompanyName = "Hufflepuff";

// Print tracking state of the new customer
Console.WriteLine($"The tracking state of the new customer is {context.Entry(newTrackedCustomer).State}");

// Save the changes
await context.SaveChangesAsync();

// Print tracking state of the new customer
Console.WriteLine($"The tracking state of the new customer is {context.Entry(newTrackedCustomer).State}");

// =====================================================

await context.Customers.Where(c => c.CompanyName == "Slytherin" || c.CompanyName == "Ravenclaw").ExecuteDeleteAsync();

// Add two customers within a transaction
await using (var transaction = await context.Database.BeginTransactionAsync())
{
    try
    {
        var newCustomer1 = new Customer(0, "Slytherin", "UK", "Diagon Alley");
        var newCustomer2 = new Customer(0, "Ravenclaw", "UKK", "Diagon Alley");

        await context.Customers.AddAsync(newCustomer1);
        await context.Customers.AddAsync(newCustomer2);

        await context.SaveChangesAsync();

        await transaction.CommitAsync();
    }
    catch (Exception)
    {
        await transaction.RollbackAsync();
        Console.WriteLine("Rolling back the transaction");
    }
}

// Make sure there are no customers named "Slytherin"
var customersNamedSlytherin = await context.Customers.Where(c => c.CompanyName == "Slytherin").ToListAsync();
if (customersNamedSlytherin.Count == 0)
{
    Console.WriteLine("There are no customers named Slytherin");
}
