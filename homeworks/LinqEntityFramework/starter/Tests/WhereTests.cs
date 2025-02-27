using FluentAssertions;
using Exercises;

namespace Tests;

public class WhereTests //(DataContextFixture fixture) : IClassFixture<DataContextFixture>, IAsyncDisposable
{
    //private readonly ApplicationDataContext context = fixture.Context;

    [Fact]
    public async Task BasicWhereOrderHeaders()
    {
        var where = new Where();//(context);
        var result = await where.BasicWhereOrderHeaders();

        Where.BasicWhereOrderHeaderResult[] expected =
        [
            new(4, "Soylent Corp", 15, new DateOnly(2024, 8, 20)),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task BasicWhereCustomers()
    {
        var where = new Where();//(context);
        var result = await where.BasicWhereCustomers();

        Where.BasicWhereCustomerResult[] expected =
        [
            new(1, "Acme Corp"),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task CustomersWithOrdersOutsideHomeCountry()
    {
        var where = new Where();//(context);
        var result = (await where.CustomersWithOrdersOutsideHomeCountry()).OrderBy(c => c.ID);

        Where.CustomersWithOrdersOutsideHomeCountryResult[] expected =
        [
            new(1, "Acme Corp"),
            new(2, "Globex Corp"),
            new(3, "Initech"),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task CustomersWithOrdersOutsideHomeCountryAdvanced()
    {
        var where = new Where();//(context);
        var result = (await where.CustomersWithOrdersOutsideHomeCountryAdvanced()).OrderBy(c => c.ID);

        Where.CustomersWithOrdersOutsideHomeCountryAdvancedResult[] expected =
        [
            new(1, "Acme Corp", "US", ["CAN", "MEX", "US"]),
            new(2, "Globex Corp", "DE", ["AT", "DE"]),
            new(3, "Initech", "MEX", ["MEX", "US"]),
        ];

        result.Should().BeEquivalentTo(expected);
    }

    /*
    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
    */
}
