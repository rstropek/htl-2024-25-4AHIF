namespace Exercises;

public class Ordering
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}