using FluentAssertions;
using Exercises;

namespace Tests;

public class WhereTests
{
    [Fact]
    public void BasicWhereOrderHeaders()
    {
        var result = Where.BasicWhereOrderHeaders();

        Where.BasicWhereOrderHeaderResult[] expected =
        [
            new(4, "Soylent Corp", 15, new DateOnly(2024, 8, 20)),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CustomersWithOrdersOutsideHomeCountry()
    {
        var result = Where.CustomersWithOrdersOutsideHomeCountry().OrderBy(c => c.ID);

        Where.CustomersWithOrdersOutsideHomeCountryResult[] expected =
        [
            new(1, "Acme Corp"),
            new(2, "Globex Corp"),
            new(3, "Initech"),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CustomersWithOrdersOutsideHomeCountryAdvanced()
    {
        var result = Where.CustomersWithOrdersOutsideHomeCountryAdvanced().OrderBy(c => c.ID);

        Where.CustomersWithOrdersOutsideHomeCountryAdvancedResult[] expected =
        [
            new(1, "Acme Corp", "US", ["CAN", "MEX", "US"]),
            new(2, "Globex Corp", "DE", ["AT", "DE"]),
            new(3, "Initech", "MEX", ["MEX", "US"]),
        ];

        result.Should().BeEquivalentTo(expected);
    }
}
