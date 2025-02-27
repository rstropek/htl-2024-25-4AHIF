using Microsoft.EntityFrameworkCore;

namespace Exercises;

public class Select(ApplicationDataContext context)
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
        return await context.Customers.Select(c => new BasicSelectCustomerResult(c.ID, c.CompanyName, c.CountryIsoCode)).ToArrayAsync();
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
        return await context.Customers.Select(c => new { c.ID, c.CompanyName, c.CountryIsoCode }).ToArrayAsync();
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
        return await context.Orders.Select(oh => new BasicSelectOrderHeadersResult(oh.ID, oh.OrderDate, oh.Customer.ID, oh.Customer.CompanyName))
            .ToArrayAsync();
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
        return await context.Customers.Select(
            c => new SelectWithAggregateCustomerOrderCountResult(c.ID, c.CompanyName, c.Orders.Count))
            .ToArrayAsync();
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
        var customerWithOrders = await context.OrderLines
            .Select(ol => new { ID = ol.Order.CustomerID, ol.Order.Customer.CompanyName, Revenue = ol.Quantity * ol.UnitPrice })
            .GroupBy(ol => new { ol.ID, ol.CompanyName })
            .Select(g => new SelectWithAggregateCustomerTotalRevenueResult(g.Key.ID, g.Key.CompanyName, g.Sum(ol => ol.Revenue)))
            .ToArrayAsync();

        var customerWithoutOrders = await context.Customers
                .Where(c => c.Orders.Count == 0)
                .Select(c => new SelectWithAggregateCustomerTotalRevenueResult(c.ID, c.CompanyName, 0))
                .ToArrayAsync();

        return [.. customerWithOrders.Concat(customerWithoutOrders).OrderByDescending(c => c.TotalRevenue)];
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
        return await context.Orders
            .Where(o => o.OrderDate.Year == 2024 && o.OrderDate.Month == 1)
            .GroupBy(o => new { o.Customer.ID, o.Customer.CompanyName })
            .Select(g => new SelectCustomersWithOrderCountJanuary2024Result(g.Key.ID, g.Key.CompanyName, g.Count()))
            .ToArrayAsync();
    }
}