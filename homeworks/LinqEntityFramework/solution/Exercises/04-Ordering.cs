using Microsoft.EntityFrameworkCore;

namespace Exercises;

public class Ordering(ApplicationDataContext context)
{
    public record OrderValueTrendResult(int CustomerID, string CompanyName, int Year, int Month, decimal TotalOrderValue);

    /// <summary>
    /// Returns a list of customers with their ID, company name, year, month and total order value.
    /// </summary>
    /// <remarks>
    /// The result must be ordered by year (asc), then by month (asc), then by total order value (desc).
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderby
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.thenby
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.thenbydescending
    /// </remarks>
    public async Task<OrderValueTrendResult[]> OrderValueTrend()
    {
        var unordered = await context.OrderLines
            .Select(ol => new
            {
                ol.Order.Customer.ID,
                ol.Order.Customer.CompanyName,
                ol.Order.OrderDate.Year,
                ol.Order.OrderDate.Month,
                OrderValue = ol.Quantity * ol.UnitPrice
            })
            .GroupBy(ovt => new { ovt.ID, ovt.CompanyName, ovt.Year, ovt.Month })
            .Select(g => new OrderValueTrendResult(
                g.Key.ID,
                g.Key.CompanyName,
                g.Key.Year,
                g.Key.Month,
                g.Sum(ovt => ovt.OrderValue)
            ))
            .ToArrayAsync();

        return [.. unordered
            .OrderBy(ovt => ovt.Year)
            .ThenBy(ovt => ovt.Month)
            .ThenByDescending(ovt => ovt.TotalOrderValue)];
    }

    public record TopCustomersResult(int CustomerID, string CompanyName, decimal TotalOrderValue);

    /// <summary>
    /// Returns the top 2 customers by sum of total order value with their ID, company name and total order value.
    /// </summary>
    /// <remarks>
    /// The result must be ordered by total order value in descending order.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderbydescending
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.take
    /// </remarks>
    public async Task<TopCustomersResult[]> TopCustomers()
    {
        return [.. (await context.OrderLines
            .GroupBy(ol => new { ol.Order.Customer.ID, ol.Order.Customer.CompanyName })
            .Select(g => new TopCustomersResult(
                g.Key.ID,
                g.Key.CompanyName,
                g.Sum(ol => ol.Quantity * ol.UnitPrice)))
            .ToArrayAsync())
            .OrderByDescending(tc => tc.TotalOrderValue)
            .Take(2)];
    }
}