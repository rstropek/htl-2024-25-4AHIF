namespace Gardens.Logic;

public static class Fertility
{
    /// <summary>
    /// Get the fertility points for given garden coordinates.
    /// </summary>
    public static decimal GetFertilityPoints(GardenCoordinates coordinates)
        // Note how we can use C# pattern matching to simplify the code.
        // If you are not familiar with pattern matching, do a little research.
        => coordinates switch 
            {
                (>= 4, <= 4) => 1.5m,
                (<= 2, >= 5) => 0.5m,
                _ => 1m,
            };
}
