namespace Exercises;

public class Set
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}