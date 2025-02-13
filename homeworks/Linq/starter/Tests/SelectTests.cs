using Exercises;

namespace Tests;

public class SelectTests
{
    [Fact]
    public void BasicSelectCustomers()
    {
        var result = Select.BasicSelectCustomers().OrderBy(c => c.ID);

        Select.BasicSelectCustomerResult[] expected =
        [
            new(1, "Acme Corp", "US"),
            new(2, "Globex Corp", "DE"),
            new(3, "Initech", "MEX"),
            new(4, "Soylent Corp", "JP"),
            new(5, "Umbrella Corp", "CHE"),
        ];

        Assert.Equal(expected, result);
    }

    private record BasicSelectCustomersAnonymousResult(int ID, string CompanyName, string CountryIsoCode);
    [Fact]
    public void BasicSelectCustomersAnonymous()
    {
        var result = Select.BasicSelectCustomersAnonymous().ToArray();

        BasicSelectCustomersAnonymousResult[] expected =
        [
            new(1, "Acme Corp", "US"),
            new(2, "Globex Corp", "DE"),
            new(3, "Initech", "MEX"),
            new(4, "Soylent Corp", "JP"),
            new(5, "Umbrella Corp", "CHE"),
        ];

        Assert.Equal(expected.Length, result.Length);
        Assert.Equal(
            ["ID", "CompanyName", "CountryIsoCode"], 
            result[0].GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Select(p => p.Name));
        var converted = result.Select(r => new BasicSelectCustomersAnonymousResult(
            (int)r.GetType().GetProperty("ID")!.GetValue(r)!,
            (string)r.GetType().GetProperty("CompanyName")!.GetValue(r)!,
            (string)r.GetType().GetProperty("CountryIsoCode")!.GetValue(r)!)).OrderBy(r => r.ID);
        Assert.Equal(expected, converted);
    }

    [Fact]
    public void BasicSelectOrderHeaders()
    {
        var result = Select.BasicSelectOrderHeaders().OrderBy(oh => oh.OrderHeaderID);

        Select.BasicSelectOrderHeadersResult[] expected =
        [
            new(1, new DateOnly(2024, 1, 10), 1, "Acme Corp"),
            new(2, new DateOnly(2024, 2, 15), 1, "Acme Corp"),
            new(3, new DateOnly(2024, 4, 5), 1, "Acme Corp"),
            new(4, new DateOnly(2024, 6, 22), 1, "Acme Corp"),
            new(5, new DateOnly(2024, 9, 8), 1, "Acme Corp"),
            new(6, new DateOnly(2024, 11, 30), 1, "Acme Corp"),
            new(7, new DateOnly(2024, 12, 15), 1, "Acme Corp"),
            new(8, new DateOnly(2024, 1, 5), 2, "Globex Corp"),
            new(9, new DateOnly(2024, 3, 15), 2, "Globex Corp"),
            new(10, new DateOnly(2024, 7, 1), 2, "Globex Corp"),
            new(11, new DateOnly(2024, 2, 20), 3, "Initech"),
            new(12, new DateOnly(2024, 5, 10), 3, "Initech"),
            new(13, new DateOnly(2024, 10, 15), 3, "Initech"),
            new(14, new DateOnly(2024, 4, 1), 4, "Soylent Corp"),
            new(15, new DateOnly(2024, 8, 20), 4, "Soylent Corp"),
            new(16, new DateOnly(2024, 12, 1), 4, "Soylent Corp")
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SelectWithAggregateCustomerOrderCount()
    {
        var result = Select.SelectWithAggregateCustomerOrderCount().OrderBy(c => c.ID);

        Select.SelectWithAggregateCustomerOrderCountResult[] expected =
        [
            new(1, "Acme Corp", 7),
            new(2, "Globex Corp", 3),
            new(3, "Initech", 3),
            new(4, "Soylent Corp", 3),
            new(5, "Umbrella Corp", 0)
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SelectWithAggregateCustomerTotalRevenue()
    {
        var result = Select.SelectWithAggregateCustomerTotalRevenue();

        Select.SelectWithAggregateCustomerTotalRevenueResult[] expected =
        [
            new(2, "Globex Corp", 69936.72m),
            new(1, "Acme Corp", 63144.06m),
            new(3, "Initech", 55604.66m),
            new(4, "Soylent Corp", 33771.28m),
            new(5, "Umbrella Corp", 0.00m)
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SelectCustomersWithOrderCountJanuary2024()
    {
        var result = Select.SelectCustomersWithOrderCountJanuary2024().OrderBy(c => c.ID);

        Select.SelectCustomersWithOrderCountJanuary2024Result[] expected =
        [
            new(1, "Acme Corp", 1),
            new(2, "Globex Corp", 1),
        ];

        Assert.Equal(expected, result);
    }

}
