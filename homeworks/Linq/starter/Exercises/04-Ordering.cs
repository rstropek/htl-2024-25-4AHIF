namespace Exercises;

public static class Ordering
{
    public record OrderValueTrendResult(int CustomerID, string CompanyName, int Year, int Month, decimal TotalOrderValue);

    /// <summary>
    /// Returns a list of customers with their ID, company name, year, month and total order value.
    /// </summary>
    /// <remarks>
    /// The result must be ordered by year (asc), then by month (asc), then by total order value (desc).
    /// </remarks>
    public static IEnumerable<OrderValueTrendResult> OrderValueTrend()
    {
        throw new NotImplementedException();
    }
}