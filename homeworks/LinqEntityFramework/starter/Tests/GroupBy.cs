using Exercises;

namespace Tests;

public class GroupByTests //(DataContextFixture fixture) : IClassFixture<DataContextFixture>, IAsyncDisposable
{
    //private readonly ApplicationDataContext context = fixture.Context;

    [Fact]
    public async Task OrdersGroupedByMonthNumberOfOrders()
    {
        var groupBy = new GroupBy();//(context);
        var result = (await groupBy.OrdersGroupedByMonthNumberOfOrders()).OrderBy(r => r.FirstOfMonth);

        GroupBy.OrdersGroupedByMonthNumberOfOrdersResult[] expected =
        [
            new(new DateOnly(2024, 1, 1), 2),
            new(new DateOnly(2024, 2, 1), 2),
            new(new DateOnly(2024, 3, 1), 1),
            new(new DateOnly(2024, 4, 1), 2),
            new(new DateOnly(2024, 5, 1), 1),
            new(new DateOnly(2024, 6, 1), 1),
            new(new DateOnly(2024, 7, 1), 1),
            new(new DateOnly(2024, 8, 1), 1),
            new(new DateOnly(2024, 9, 1), 1),
            new(new DateOnly(2024, 10, 1), 1),
            new(new DateOnly(2024, 11, 1), 1),
            new(new DateOnly(2024, 12, 1), 2),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task TotalOrderValueByCustomer()
    {
        var groupBy = new GroupBy();//(context);
        var result = (await groupBy.TotalOrderValueByCustomer()).OrderBy(r => r.CompanyID);

        GroupBy.TotalOrderValueByCustomerResult[] expected =
        [
            new(1, "Acme Corp", 63144.06m),
            new(2, "Globex Corp", 69936.72m),
            new(3, "Initech", 55604.66m),
            new(4, "Soylent Corp", 33771.28m),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task OrderStatsByCustomer()
    {
        var groupBy = new GroupBy();//(context);
        var result = (await groupBy.OrderStatsByCustomer()).OrderBy(r => r.Customer.CompanyID);

        GroupBy.OrderStatsByCustomerResult[] expected =
        [
            new(new(1, "Acme Corp"), 9020.58m, 63144.06m, 7),
            new(new(2, "Globex Corp"), 23312.24m, 69936.72m, 3),
            new(new(3, "Initech"), 18534.8866666667m, 55604.66m, 3),
            new(new(4, "Soylent Corp"), 11257.0933333333m, 33771.28m, 3),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task OrderStatsByAllCustomer()
    {
        var groupBy = new GroupBy();//(context);
        var result = (await groupBy.OrderStatsByAllCustomer()).OrderBy(r => r.Customer.CompanyID);

        GroupBy.OrderStatsByCustomerResult[] expected =
        [
            new(new(1, "Acme Corp"), 9020.58m, 63144.06m, 7),
            new(new(2, "Globex Corp"), 23312.24m, 69936.72m, 3),
            new(new(3, "Initech"), 18534.8866666667m, 55604.66m, 3),
            new(new(4, "Soylent Corp"), 11257.0933333333m, 33771.28m, 3),
            new(new(5, "Umbrella Corp"), 0m, 0m, 0),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task OrderStatsByLargeCustomer()
    {
        var groupBy = new GroupBy();//(context);
        var result = (await groupBy.OrderStatsByLargeCustomer()).OrderBy(r => r.Customer.CompanyID);

        GroupBy.OrderStatsByCustomerResult[] expected =
        [
            new(new(1, "Acme Corp"), 9020.58m, 63144.06m, 7),
            new(new(2, "Globex Corp"), 23312.24m, 69936.72m, 3),
        ];

        Assert.Equal(expected, result);
    }
    
    /*
    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
    */
}