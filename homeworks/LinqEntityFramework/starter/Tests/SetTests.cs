using Exercises;

namespace Tests;

public class SetTests //(DataContextFixture fixture) : IClassFixture<DataContextFixture>, IAsyncDisposable
{
    //private readonly ApplicationDataContext context = fixture.Context;

    [Fact]
    public async Task CustomerNotBuyingFeb2024()
    {
        var set = new Set();//(context);
        var result = await set.CustomerNotBuyingFeb2024();

        Set.CustomerNotBuyingFeb2024Result[] expected =
        [
            new(2, "Globex Corp"),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task OrderLinesAboveAverage()
    {
        var set = new Set();//(context);
        var result = (await set.OrderLinesAboveAverage()).OrderBy(ol => ol.ID);

        Set.OrderLinesAboveAverageResult[] expected =
        [
            new(1, "VW3732", 1, 4990m, 4918.5m),
            new(5, "VW3733-X", 15, 89.99m, 89.6175m),
            new(6, "VW3734", 3, 2999.99m, 1724.998m),
            new(8, "VW3734-LITE", 20, 149.99m, 144.495m),
            new(20, "VW3733-X", 20, 89.99m, 89.6175m),
            new(21, "VW3734", 2, 2750m, 1724.998m),
            new(22, "VW3734-PRO", 5, 499.99m, 472.49m),
            new(25, "VW3732-A", 6, 44.99m, 37.24m),
            new(26, "VW3733", 8, 4499.99m, 3837.245m),
            new(27, "VW3733-X", 4, 89.99m, 89.6175m),
            new(29, "VW3734", 1, 2999.99m, 1724.998m),
            new(31, "VW3732", 4, 4999m, 4918.5m),
            new(33, "VW3733", 2, 3999m, 3837.245m),
            new(35, "VW3733-Y", 10, 21.99m, 20.99m),
            new(36, "VW3734", 1, 2875m, 1724.998m),
            new(37, "VW3734-PRO", 4, 489.99m, 472.49m)
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task RepeatedBuying()
    {
        var set = new Set();//(context);
        var result = (await set.RepeatedBuying()).OrderBy(r => r.CustomerID).ThenBy(r => r.Year1).ThenBy(r => r.Month1).ThenBy(r => r.Year2).ThenBy(r => r.Month2);

        Set.RepeatedBuyingResult[] expected =
        [
            new(1, "Acme Corp", 2024, 1, 2024, 2),
            new(1, "Acme Corp", 2024, 11, 2024, 12),
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