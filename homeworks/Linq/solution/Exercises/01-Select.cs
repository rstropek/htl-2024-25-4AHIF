namespace Exercises;

public static class Select
{
    public record BasicSelectCustomerResult(int ID, string CompanyName, string CountryIsoCode);

    /// <summary>
    /// Returns a list of customers with their ID, company name and country ISO code.
    /// </summary>
    /// <remarks>
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// </remarks>
    public static IEnumerable<BasicSelectCustomerResult> BasicSelectCustomers()
    {
        return SampleData.Customers.Select(c => new BasicSelectCustomerResult(c.ID, c.CompanyName, c.CountryIsoCode));
    }

    /// <summary>
    /// Returns a list of customers with their ID, company name and country ISO code.
    /// </summary>
    /// <remarks>
    /// In contrast to <see cref="BasicSelectCustomers"/>, this method returns an **anonymous type**.
    /// See also https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// </remarks>
    public static IEnumerable<object> BasicSelectCustomersAnonymous()
    {
        return SampleData.Customers.Select(c => new { c.ID, c.CompanyName, c.CountryIsoCode });
    }

    public record BasicSelectOrderHeadersResult(int OrderHeaderID, DateOnly OrderDate, int CustomerID, string CompanyName);

    /// <summary>
    /// Returns a list of order headers with their ID, order date, customer ID and company name.
    /// </summary>
    /// <remarks>
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// </remarks>
    public static IEnumerable<BasicSelectOrderHeadersResult> BasicSelectOrderHeaders()
    {
        return SampleData.OrderHeaders.Select(
            oh => new BasicSelectOrderHeadersResult(oh.ID, oh.OrderDate, oh.Customer.ID, oh.Customer.CompanyName));
    }

    public record SelectWithAggregateCustomerOrderCountResult(int ID, string CompanyName, int OrderCount);

    /// <summary>
    /// Returns a list of customers with their ID, company name and the number of orders they have placed.
    /// </summary>
    /// <remarks>
    /// Customers that did not place any orders must also be in the result with an order count of 0.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count
    /// </remarks>
    public static IEnumerable<SelectWithAggregateCustomerOrderCountResult> SelectWithAggregateCustomerOrderCount()
    {
        return SampleData.Customers.Select(
            c => new SelectWithAggregateCustomerOrderCountResult(c.ID, c.CompanyName, c.Orders.Count));
    }

    public record SelectWithAggregateCustomerTotalRevenueResult(int ID, string CompanyName, decimal TotalRevenue);

    /// <summary>
    /// Returns a list of customers with their ID, company name and the total revenue of all their orders.
    /// </summary>
    /// <remarks>
    /// The total revenue is the sum of quantity * unit price for each order line.
    /// Customers that did not place any orders must also be in the result with a total revenue of 0.
    /// The result must be ordered by total revenue in descending order.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.selectmany
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.sum
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderbydescending
    /// </remarks>
    public static IEnumerable<SelectWithAggregateCustomerTotalRevenueResult> SelectWithAggregateCustomerTotalRevenue()
    {
        return SampleData.Customers.Select(
            c => new SelectWithAggregateCustomerTotalRevenueResult(
                c.ID,
                c.CompanyName,
                c.Orders.SelectMany(o => o.OrderLines).Sum(ol => ol.Quantity * ol.UnitPrice)))
            .OrderByDescending(c => c.TotalRevenue);
    }

    public record SelectCustomersWithOrderCountJanuary2024Result(int ID, string CompanyName, int OrderCount);

    /// <summary>
    /// Returns a list of customers with their ID, company name and the number of orders they have placed in January 2024.
    /// </summary>
    /// <remarks>
    /// Customers that did not place any orders in January 2024 must not be in the result.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.count
    /// </remarks>
    public static IEnumerable<SelectCustomersWithOrderCountJanuary2024Result> SelectCustomersWithOrderCountJanuary2024()
    {
        return SampleData.Customers.Select(
            c => new SelectCustomersWithOrderCountJanuary2024Result(
                c.ID,
                c.CompanyName,
                c.Orders.Count(o => o.OrderDate.Year == 2024 && o.OrderDate.Month == 1)))
            .Where(c => c.OrderCount > 0);
    }
}