namespace Exercises;

public static class Ordering
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
    public static IEnumerable<OrderValueTrendResult> OrderValueTrend()
    {
        return SampleData.OrderHeaders
            .Select(oh => new OrderValueTrendResult(
                oh.Customer.ID,
                oh.Customer.CompanyName,
                oh.OrderDate.Year,
                oh.OrderDate.Month,
                oh.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice)))
            .OrderBy(ovt => ovt.Year)
            .ThenBy(ovt => ovt.Month)
            .ThenByDescending(ovt => ovt.TotalOrderValue);
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
    public static IEnumerable<TopCustomersResult> TopCustomers()
    {
        return SampleData.Customers
            .Select(c => new TopCustomersResult(c.ID, c.CompanyName, c.Orders.Sum(o => o.OrderLines.Sum(ol => ol.Quantity * ol.UnitPrice))))
            .OrderByDescending(tc => tc.TotalOrderValue)
            .Take(2);
    }
}