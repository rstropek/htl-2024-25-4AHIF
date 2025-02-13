using Exercises;

namespace Tests;

public class OrderingTests
{
    [Fact]
    public void OrderValueTrend()
    {
        var result = Ordering.OrderValueTrend();

        Ordering.OrderValueTrendResult[] expected =
        {
            new(2, "Globex Corp", 2024, 1, 24551.97m),
            new(1, "Acme Corp", 2024, 1, 6334.90m),
            new(3, "Initech", 2024, 2, 14654.94m),
            new(1, "Acme Corp", 2024, 2, 8349.83m),
            new(2, "Globex Corp", 2024, 3, 35299.80m),
            new(4, "Soylent Corp", 2024, 4, 20275.92m),
            new(1, "Acme Corp", 2024, 4, 15599.69m),
            new(3, "Initech", 2024, 5, 36599.76m),
            new(1, "Acme Corp", 2024, 6, 9499.94m),
            new(2, "Globex Corp", 2024, 7, 10084.95m),
            new(4, "Soylent Corp", 2024, 8, 8660.40m),
            new(1, "Acme Corp", 2024, 9, 10159.84m),
            new(3, "Initech", 2024, 10, 4349.96m),
            new(1, "Acme Corp", 2024, 11, 16199.85m),
            new(4, "Soylent Corp", 2024, 12, 4834.96m),
            new(1, "Acme Corp", 2024, 12, -2999.99m)
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public void TopCustomers()
    {
        var result = Ordering.TopCustomers();

        Ordering.TopCustomersResult[] expected =
        {
            new(2, "Globex Corp", 69936.72m),
            new(1, "Acme Corp", 63144.06m),
        };

        Assert.Equal(expected, result);
    }
}