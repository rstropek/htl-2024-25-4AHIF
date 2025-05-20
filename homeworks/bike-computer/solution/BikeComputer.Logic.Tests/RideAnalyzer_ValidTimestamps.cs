namespace BikeComputer.Logic.Tests;

public class RideAnalyzer_ValidTimestamps
{
    [Fact]
    public void AreValidTimestamps_Valid()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 10, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 10, 10, 0, 1, TimeSpan.Zero),
            new(2025, 5, 10, 10, 0, 5, TimeSpan.Zero)
        };

        // Act
        bool result = RideAnalyzer.AreValidTimestamps(timestamps);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValidTimestamps_SingleTimestamp_Valid()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 10, 10, 0, 0, TimeSpan.Zero)
        };

        // Act
        bool result = RideAnalyzer.AreValidTimestamps(timestamps);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValidTimestamps_EmptyList_Valid()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>();

        // Act
        bool result = RideAnalyzer.AreValidTimestamps(timestamps);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AreValidTimestamps_Invalid_SameTimestamp()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 10, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 10, 10, 0, 0, TimeSpan.Zero)  // Same timestamp
        };

        // Act
        bool result = RideAnalyzer.AreValidTimestamps(timestamps);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AreValidTimestamps_Invalid_DescendingOrder()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 10, 10, 0, 5, TimeSpan.Zero),
            new(2025, 5, 10, 10, 0, 3, TimeSpan.Zero),  // Earlier time
            new(2025, 5, 10, 10, 0, 7, TimeSpan.Zero)
        };

        // Act
        bool result = RideAnalyzer.AreValidTimestamps(timestamps);

        // Assert
        Assert.False(result);
    }
}
