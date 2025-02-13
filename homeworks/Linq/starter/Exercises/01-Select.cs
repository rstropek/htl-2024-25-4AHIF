namespace Exercises;

public static class Select
{
    public record BasicSelectCustomerResult(int ID, string CompanyName, string CountryIsoCode);

    /// <summary>
    /// Returns a list of customers with their ID, company name and country ISO code.
    /// </summary>
    public static IEnumerable<BasicSelectCustomerResult> BasicSelectCustomers()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a list of customers with their ID, company name and country ISO code.
    /// </summary>
    /// <remarks>
    /// In contrast to BasicSelectCustomers, this method returns an **anonymous type**.
    /// </remarks>
    public static IEnumerable<object> BasicSelectCustomersAnonymous()
    {
        throw new NotImplementedException();
    }

    public record BasicSelectOrderHeadersResult(int OrderHeaderID, DateOnly OrderDate, int CustomerID, string CompanyName);

    /// <summary>
    /// Returns a list of order headers with their ID, order date, customer ID and company name.
    /// </summary>
    public static IEnumerable<BasicSelectOrderHeadersResult> BasicSelectOrderHeaders()
    {
        throw new NotImplementedException();
    }

    public record SelectWithAggregateCustomerOrderCountResult(int ID, string CompanyName, int OrderCount);

    /// <summary>
    /// Returns a list of customers with their ID, company name and the number of orders they have placed.
    /// </summary>
    /// <remarks>
    /// Customers that did not place any orders must also be in the result with an order count of 0.
    /// </remarks>
    public static IEnumerable<SelectWithAggregateCustomerOrderCountResult> SelectWithAggregateCustomerOrderCount()
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
    /// </remarks>
    public static IEnumerable<SelectWithAggregateCustomerTotalRevenueResult> SelectWithAggregateCustomerTotalRevenue()
    {
        throw new NotImplementedException();
    }

    public record SelectCustomersWithOrderCountJanuary2024Result(int ID, string CompanyName, int OrderCount);

    /// <summary>
    /// Returns a list of customers with their ID, company name and the number of orders they have placed in January 2024.
    /// </summary>
    /// <remarks>
    /// Customers that did not place any orders in January 2024 must not be in the result.
    /// </remarks>
    public static IEnumerable<SelectCustomersWithOrderCountJanuary2024Result> SelectCustomersWithOrderCountJanuary2024()
    {
        throw new NotImplementedException();
    }
}