using Microsoft.EntityFrameworkCore;

namespace Exercises;

public class Where(ApplicationDataContext context)
{
    public record BasicWhereOrderHeaderResult(int CustomerID, string CompanyName, int OrderID, DateOnly OrderDate);

    /// <summary>
    /// Returns a list of order headers for orders placed by customers in Japan in August 2024 that contain at least
    /// one order line with a product code starting with "VW3733".
    /// </summary>
    /// <remarks>
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// </remarks>
    public async Task<BasicWhereOrderHeaderResult[]> BasicWhereOrderHeaders()
    {
        return await context.Orders
            .Where(oh => oh.Customer.CountryIsoCode == "JP")
            .Where(oh => oh.OrderDate.Year == 2024 && oh.OrderDate.Month == 8)
            .Where(oh => oh.OrderLines.Any(ol => ol.ProductCode.StartsWith("VW3733")))
            .Select(oh => new BasicWhereOrderHeaderResult(oh.Customer.ID, oh.Customer.CompanyName, oh.ID, oh.OrderDate))
            .ToArrayAsync();
    }

    public record BasicWhereCustomerResult(int ID, string CompanyName);

    /// <summary>
    /// Returns a list of customers that have received a credit note (order line with a negative unit price).
    /// </summary>
    /// <remarks>
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// </remarks>
    public async Task<BasicWhereCustomerResult[]> BasicWhereCustomers()
    {
        return await context.Customers
            .Where(c => c.Orders.Any(o => o.OrderLines.Any(ol => ol.UnitPrice < 0)))
            .Select(c => new BasicWhereCustomerResult(c.ID, c.CompanyName))
            .ToArrayAsync();
    }

    public record CustomersWithOrdersOutsideHomeCountryResult(int ID, string CompanyName);

    /// <summary>
    /// Returns a list of customers that have placed orders with a delivery country different from their home country.
    /// </summary>
    /// <remarks>
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// </remarks>
    public async Task<CustomersWithOrdersOutsideHomeCountryResult[]> CustomersWithOrdersOutsideHomeCountry()
    {
        return await context.Customers
            .Where(c => c.Orders.Any(o => o.DeliveryCountryIsoCode != c.CountryIsoCode))
            .Select(c => new CustomersWithOrdersOutsideHomeCountryResult(c.ID, c.CompanyName))
            .ToArrayAsync();
    }

    public record CustomersWithOrdersOutsideHomeCountryAdvancedResult(int ID, string CompanyName, string CountryIsoCode, IEnumerable<string> DeliveryCountryIsoCodes);

    /// <summary>
    /// Returns a list of customers that have placed any orders with a delivery country different from their home country.
    /// </summary>
    /// <remarks>
    /// <see cref="CustomersWithOrdersOutsideHomeCountryAdvancedResult.DeliveryCountryIsoCodes"/> must contain all the distinct 
    /// delivery country ISO codes for each customer ordered alphabetically.
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.distinct
    /// See also https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.order
    /// </remarks>
    public async Task<CustomersWithOrdersOutsideHomeCountryAdvancedResult[]> CustomersWithOrdersOutsideHomeCountryAdvanced()
    {
        // First, get the customers with foreign orders
        var customers = await context.Customers
            .Where(c => c.Orders.Any(o => o.DeliveryCountryIsoCode != c.CountryIsoCode))
            .Select(c => new 
            {
                c.ID,
                c.CompanyName,
                c.CountryIsoCode
            })
            .ToListAsync();
        
        // Then, for each customer, get their foreign delivery countries in a separate query
        var result = new List<CustomersWithOrdersOutsideHomeCountryAdvancedResult>();
        
        foreach (var customer in customers)
        {
            var foreignDeliveryCountries = await context.Orders
                .Where(o => o.CustomerID == customer.ID)
                .Select(o => o.DeliveryCountryIsoCode)
                .Distinct()
                .OrderBy(code => code)
                .ToListAsync();
            
            result.Add(new CustomersWithOrdersOutsideHomeCountryAdvancedResult(
                customer.ID,
                customer.CompanyName,
                customer.CountryIsoCode,
                foreignDeliveryCountries));
        }
        
        return [.. result];
    }
}