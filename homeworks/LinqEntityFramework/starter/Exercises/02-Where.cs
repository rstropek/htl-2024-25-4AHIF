namespace Exercises;

public class Where
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}