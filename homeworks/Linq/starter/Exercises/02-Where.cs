namespace Exercises;

public static class Where
{
    public record BasicWhereOrderHeaderResult(int CustomerID, string CompanyName, int OrderID, DateOnly OrderDate);

    /// <summary>
    /// Returns a list of order headers for orders placed by customers in Japan in August 2024 that contain at least
    /// one order line with a product code starting with "VW3733".
    /// </summary>
    public static IEnumerable<BasicWhereOrderHeaderResult> BasicWhereOrderHeaders()
    {
        throw new NotImplementedException();
    }

    public record CustomersWithOrdersOutsideHomeCountryResult(int ID, string CompanyName);

    /// <summary>
    /// Returns a list of customers that have placed orders with a delivery country different from their home country.
    /// </summary>
    public static IEnumerable<CustomersWithOrdersOutsideHomeCountryResult> CustomersWithOrdersOutsideHomeCountry()
    {
        throw new NotImplementedException();
    }

    public record CustomersWithOrdersOutsideHomeCountryAdvancedResult(int ID, string CompanyName, string CountryIsoCode, string[] DeliveryCountryIsoCodes);

    /// <summary>
    /// Returns a list of customers that have placed any orders with a delivery country different from their home country.
    /// </summary>
    /// <remarks>
    /// DeliveryCountryIsoCodes must contain all the distinct delivery country ISO codes for each customer ordered alphabetically.
    /// </remarks>
    public static IEnumerable<CustomersWithOrdersOutsideHomeCountryAdvancedResult> CustomersWithOrdersOutsideHomeCountryAdvanced()
    {
        throw new NotImplementedException();
    }

}