namespace Exercises;

public static class Set
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
    public static IEnumerable<CustomerNotBuyingFeb2024Result> CustomerNotBuyingFeb2024()
    {
        return SampleData.Customers
            .Where(c => !c.Orders.Any(o => o.OrderDate.Year == 2024 && o.OrderDate.Month == 2))
            .Intersect(SampleData.Customers
                .Where(c => c.Orders.Any(o => o.OrderDate.Year == 2024 && o.OrderDate.Month == 1)))
            .Select(c => new CustomerNotBuyingFeb2024Result(c.ID, c.CompanyName));
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
    public static IEnumerable<OrderLinesAboveAverageResult> OrderLinesAboveAverage()
    {
        var averageUnitPrices = SampleData.OrderLines
            .GroupBy(ol => ol.ProductCode)
            .ToDictionary(g => g.Key, g => g.Average(ol => ol.UnitPrice));

        return SampleData.OrderLines
            .Where(ol => ol.UnitPrice > averageUnitPrices[ol.ProductCode])
            .Select(ol => new OrderLinesAboveAverageResult(ol.ID, ol.ProductCode, ol.Quantity, ol.UnitPrice, averageUnitPrices[ol.ProductCode]));
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
    public static IEnumerable<RepeatedBuyingResult> RepeatedBuying()
    {
        return SampleData.OrderHeaders
            .SelectMany(order1 =>
                SampleData.OrderHeaders
                    .Where(order2 => 
                        order1.Customer.ID == order2.Customer.ID && 
                        ((order2.OrderDate.Year == order1.OrderDate.Year && order2.OrderDate.Month == order1.OrderDate.Month + 1) ||
                        (order2.OrderDate.Year == order1.OrderDate.Year + 1 && order1.OrderDate.Month == 12 && order2.OrderDate.Month == 1)))
                    .Select(order2 => new RepeatedBuyingResult(
                        order1.Customer.ID,
                        order1.Customer.CompanyName,
                        order1.OrderDate.Year,
                        order1.OrderDate.Month,
                        order2.OrderDate.Year,
                        order2.OrderDate.Month)))
            .Distinct();
    }
}