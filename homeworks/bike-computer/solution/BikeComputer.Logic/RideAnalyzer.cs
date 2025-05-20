[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("BikeComputer.Logic.Tests")]

namespace BikeComputer.Logic;


public static class RideAnalyzer
{
    private const double STOP_THRESHOLD_SECONDS = 3.0;

    internal static bool AreValidTimestamps(IReadOnlyList<DateTimeOffset> timestamps)
        => timestamps.Select((t, i) => i == 0 || t > timestamps[i - 1]).All(x => x);

    public static AnalysisResult AnalyzeRide(IReadOnlyList<DateTimeOffset> timestamps, int circumference_mm)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(timestamps.Count, 2, nameof(timestamps));
        if (!AreValidTimestamps(timestamps))
        {
            throw new ArgumentException("Timestamps must be in ascending order.");
        }

        var rideDuration = (timestamps[^1] - timestamps[0]).TotalSeconds;
        var rideDistance = CalculateDistance(timestamps, circumference_mm);
        var avgSpeed = CalculateAverageSpeed(rideDistance, rideDuration);
        var numberOfStops = CalculateNumberOfStops(timestamps);
        var totalStopTime = CalculateTotalStopTime(timestamps);

        return new AnalysisResult(
            (float)rideDuration,
            (float)rideDistance,
            (float)avgSpeed,
            numberOfStops,
            (float)totalStopTime
        );
    }

    internal static double CalculateDistance(IReadOnlyList<DateTimeOffset> timestamps, int circumference_mm)
        => (timestamps.Count - 1) * circumference_mm / 1000.0;

    internal static double CalculateAverageSpeed(double distance_m, double duration_s)
        => distance_m / duration_s * 3.6;

    internal static int CalculateNumberOfStops(IReadOnlyList<DateTimeOffset> timestamps)
    {
        int stops = 0;

        for (int i = 1; i < timestamps.Count; i++)
        {
            double timeDifference = (timestamps[i] - timestamps[i - 1]).TotalSeconds;
            if (timeDifference >= STOP_THRESHOLD_SECONDS)
            {
                stops++;
            }
        }

        return stops;
    }

    internal static double CalculateTotalStopTime(IReadOnlyList<DateTimeOffset> timestamps)
    {
        double totalStopTime = 0;

        for (int i = 1; i < timestamps.Count; i++)
        {
            double timeDifference = (timestamps[i] - timestamps[i - 1]).TotalSeconds;
            if (timeDifference >= STOP_THRESHOLD_SECONDS)
            {
                totalStopTime += timeDifference;
            }
        }

        return totalStopTime;
    }

    public record AnalysisResult(
        float RideDuration_s,
        float RideDistance_m,
        float AvgSpeed_kmh,
        int NumberOfStops,
        float TotalStopTime_s
    );
}
