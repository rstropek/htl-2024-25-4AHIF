namespace Exercises;

public class Select
{
    public record BasicSelectCustomerResult(int ID, string CompanyName, string CountryIsoCode);

    /// <summary>
    /// Returns a list of customers with their ID, company name and country ISO code.
    /// </summary>
    /// <remarks>
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// </remarks>
    public async Task<BasicSelectCustomerResult[]> BasicSelectCustomers()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a list of customers with their ID, company name and country ISO code.
    /// </summary>
    /// <remarks>
    /// In contrast to <see cref="BasicSelectCustomers"/>, this method returns an **anonymous type**.
    /// See also https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// </remarks>
    public async Task<object[]> BasicSelectCustomersAnonymous()
    {
        throw new NotImplementedException();
    }

    public record BasicSelectOrderHeadersResult(int OrderHeaderID, DateOnly OrderDate, int CustomerID, string CompanyName);

    /// <summary>
    /// Returns a list of order headers with their ID, order date, customer ID and company name.
    /// </summary>
    /// <remarks>
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// </remarks>
    public async Task<BasicSelectOrderHeadersResult[]> BasicSelectOrderHeaders()
    {
        throw new NotImplementedException();
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
    public async Task<SelectWithAggregateCustomerOrderCountResult[]> SelectWithAggregateCustomerOrderCount()
    {
        throw new NotImplementedException();
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
    public async Task<SelectWithAggregateCustomerTotalRevenueResult[]> SelectWithAggregateCustomerTotalRevenue()
    {
        throw new NotImplementedException();
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
    public async Task<SelectCustomersWithOrderCountJanuary2024Result[]> SelectCustomersWithOrderCountJanuary2024()
    {
        throw new NotImplementedException();
    }
}