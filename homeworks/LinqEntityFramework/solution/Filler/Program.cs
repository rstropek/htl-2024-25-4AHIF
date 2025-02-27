using Exercises;

var dbContextFactory = new ApplicationDataContextFactory();
using var context = dbContextFactory.CreateDbContext([]);

Console.WriteLine("Cleaning and filling with sample data...");
await context.CleanAndFillWithSampleData();
Console.WriteLine("Done!");
