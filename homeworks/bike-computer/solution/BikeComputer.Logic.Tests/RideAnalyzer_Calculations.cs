namespace BikeComputer.Logic.Tests;

public class RideAnalyzer_Calculations
{
    [Fact]
    public void CalculateDistance_SingleRevolution_ReturnsCircumferenceInMeters()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 1, TimeSpan.Zero)
        };
        int circumference_mm = 2000; // 2 meters circumference

        // Act
        double result = RideAnalyzer.CalculateDistance(timestamps, circumference_mm);

        // Assert
        Assert.Equal(2.0, result, 3); // Expect 2 meters (2000mm / 1000)
    }

    [Fact]
    public void CalculateDistance_MultipleRevolutions_ReturnsCorrectDistance()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 1, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 2, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 3, TimeSpan.Zero)
        };
        int circumference_mm = 2000; // 2 meters circumference

        // Act
        double result = RideAnalyzer.CalculateDistance(timestamps, circumference_mm);

        // Assert
        Assert.Equal(6.0, result, 3); // Expect 6 meters (3 revolutions * 2000mm / 1000)
    }

    [Fact]
    public void CalculateAverageSpeed_StandardValues_ReturnsCorrectSpeed()
    {
        // Arrange
        double distance_m = 1000; // 1 kilometer
        double duration_s = 3600; // 1 hour

        // Act
        double result = RideAnalyzer.CalculateAverageSpeed(distance_m, duration_s);

        // Assert
        Assert.Equal(1, result, 3); // Expect 1 km/h
    }

    [Fact]
    public void CalculateNumberOfStops_NoStops_ReturnsZero()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 1, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 2, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 3, TimeSpan.Zero)
        };

        // Act
        int result = RideAnalyzer.CalculateNumberOfStops(timestamps);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateNumberOfStops_WithStops_ReturnsCorrectCount()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 1, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 5, TimeSpan.Zero), // Stop (4s gap)
            new(2025, 5, 20, 10, 0, 6, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 10, TimeSpan.Zero) // Stop (4s gap)
        };

        // Act
        int result = RideAnalyzer.CalculateNumberOfStops(timestamps);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void CalculateNumberOfStops_StopAtThreshold_ReturnsCorrectCount()
    {
        // Arrange - Create timestamps with exactly 3 second difference (threshold value)
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 3, TimeSpan.Zero), // Exactly 3s gap (threshold)
            new(2025, 5, 20, 10, 0, 6, TimeSpan.Zero)  // Exactly 3s gap (threshold)
        };

        // Act
        int result = RideAnalyzer.CalculateNumberOfStops(timestamps);

        // Assert
        Assert.Equal(2, result); // Both gaps are exactly at threshold so should count as stops
    }

    [Fact]
    public void CalculateTotalStopTime_NoStops_ReturnsZero()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 1, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 2, TimeSpan.Zero)
        };

        // Act
        double result = RideAnalyzer.CalculateTotalStopTime(timestamps);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateTotalStopTime_WithStops_ReturnsTotalStopDuration()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 5, TimeSpan.Zero), // 5s gap
            new(2025, 5, 20, 10, 0, 6, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 10, TimeSpan.Zero) // 4s gap
        };

        // Act
        double result = RideAnalyzer.CalculateTotalStopTime(timestamps);

        // Assert
        Assert.Equal(9, result); // 5s + 4s = 9s total stop time
    }

    [Fact]
    public void CalculateTotalStopTime_StopsBelowThreshold_NotCounted()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 2, TimeSpan.Zero), // 2s gap (below threshold)
            new(2025, 5, 20, 10, 0, 6, TimeSpan.Zero), // 4s gap (above threshold)
            new(2025, 5, 20, 10, 0, 8, TimeSpan.Zero)  // 2s gap (below threshold)
        };

        // Act
        double result = RideAnalyzer.CalculateTotalStopTime(timestamps);

        // Assert
        Assert.Equal(4, result); // Only the 4s gap counts as stop time
    }

    [Fact]
    public void AnalyzeRide_ValidInput_ReturnsCorrectAnalysisResult()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 1, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 5, TimeSpan.Zero), // 4s gap (stop)
            new(2025, 5, 20, 10, 0, 10, TimeSpan.Zero) // 5s gap (stop)
        };
        int circumference_mm = 2000; // 2 meters circumference

        // Act
        var result = RideAnalyzer.AnalyzeRide(timestamps, circumference_mm);

        // Assert
        Assert.Equal(10.0f, result.RideDuration_s); // 10 seconds total
        Assert.Equal(6.0f, result.RideDistance_m); // 3 revolutions * 2m = 6m
        Assert.Equal(2.16f, result.AvgSpeed_kmh, 2); // 6m / 10s * 3.6 = 2.16 km/h
        Assert.Equal(2, result.NumberOfStops); // 2 stops (gaps >= 3s)
        Assert.Equal(9.0f, result.TotalStopTime_s); // 4s + 5s = 9s
    }

    [Fact]
    public void AnalyzeRide_LessThanTwoTimestamps_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero)
        };
        int circumference_mm = 2000;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            RideAnalyzer.AnalyzeRide(timestamps, circumference_mm));
        
        Assert.Equal("timestamps", exception.ParamName);
    }

    [Fact]
    public void AnalyzeRide_TimestampsNotInAscendingOrder_ThrowsArgumentException()
    {
        // Arrange
        var timestamps = new List<DateTimeOffset>
        {
            new(2025, 5, 20, 10, 0, 0, TimeSpan.Zero),
            new(2025, 5, 20, 10, 0, 5, TimeSpan.Zero),
            // This timestamp is before the previous one
            new(2025, 5, 20, 10, 0, 3, TimeSpan.Zero)
        };
        int circumference_mm = 2000;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            RideAnalyzer.AnalyzeRide(timestamps, circumference_mm));
        
        Assert.Contains("ascending order", exception.Message);
    }
}
