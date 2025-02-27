using Microsoft.EntityFrameworkCore;

namespace Exercises;

public class Set(ApplicationDataContext context)
{
    public record CustomerNotBuyingFeb2024Result(int ID, string CompanyName);

    /// <summary>
    /// Returns a list of customers that did not place any orders in February 2024 but
    /// did place any orders in January 2024.
    /// </summary>
    /// <remarks>
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.intersect
    /// </remarks>
    public async Task<CustomerNotBuyingFeb2024Result[]> CustomerNotBuyingFeb2024()
    {
        var customersNotBuyingInFeb = await context.Customers
            .Where(c => !c.Orders.Any(o => o.OrderDate.Year == 2024 && o.OrderDate.Month == 2))
            .ToListAsync();
            
        var customersBuyingInJan = await context.Customers
            .Where(c => c.Orders.Any(o => o.OrderDate.Year == 2024 && o.OrderDate.Month == 1))
            .ToListAsync();
            
        return customersNotBuyingInFeb
            .Intersect(customersBuyingInJan)
            .Select(c => new CustomerNotBuyingFeb2024Result(c.ID, c.CompanyName))
            .ToArray();
    }

    public record OrderLinesAboveAverageResult(int ID, string ProductCode, int Quantity, decimal UnitPrice, decimal AverageUnitPrice);

    /// <summary>
    /// Returns a list of order lines with their ID, product code, quantity, unit price, and average unit price
    /// that have a unit price above the average unit price of the given product code from all orders.
    /// </summary>
    /// <remarks>
    /// Solve this exercise with **two** queries. First, calculate the average unit price for each product code.
    /// Put the result of that query into a dictionary for efficient lookup. Then, use that dictionary to filter
    /// the order lines that have a unit price above the average unit price of the given product code from all orders.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.todictionary
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.average
    /// </remarks>
    public async Task<OrderLinesAboveAverageResult[]> OrderLinesAboveAverage()
    {
        var averageUnitPrices = await context.OrderLines
            .GroupBy(ol => ol.ProductCode)
            .ToDictionaryAsync(g => g.Key, g => g.Average(ol => ol.UnitPrice));

        var result = new List<OrderLinesAboveAverageResult>();
        await foreach (var orderLine in context.OrderLines)
        {
            if (orderLine.UnitPrice > averageUnitPrices[orderLine.ProductCode])
            {
                result.Add(new OrderLinesAboveAverageResult(
                    orderLine.ID, 
                    orderLine.ProductCode, 
                    orderLine.Quantity, 
                    orderLine.UnitPrice, 
                    averageUnitPrices[orderLine.ProductCode]));
            }
        }
        
        return [.. result];
    }

    public record RepeatedBuyingResult(int CustomerID, string CompanyName, int Year1, int Month1, int Year2, int Month2);

    /// <summary>
    /// Returns a list of customers that have bought something in successive months. 
    /// </summary>
    /// <remarks>
    /// The result must contain one entry per customer per successive pair of months.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.selectmany
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.distinct
    /// </remarks>
    public async Task<RepeatedBuyingResult[]> RepeatedBuying()
    {
        // This query is complex for EF Core translation, so we'll load the data first
        var orders = await context.Orders
            .Select(o => new 
            { 
                o.Customer.ID, 
                o.Customer.CompanyName, 
                o.OrderDate.Year, 
                o.OrderDate.Month 
            })
            .ToListAsync();
            
        return orders
            .SelectMany(order1 =>
                orders
                    .Where(order2 => 
                        order1.ID == order2.ID && 
                        ((order2.Year == order1.Year && order2.Month == order1.Month + 1) ||
                        (order2.Year == order1.Year + 1 && order1.Month == 12 && order2.Month == 1)))
                    .Select(order2 => new RepeatedBuyingResult(
                        order1.ID,
                        order1.CompanyName,
                        order1.Year,
                        order1.Month,
                        order2.Year,
                        order2.Month)))
            .Distinct()
            .ToArray();
    }
}