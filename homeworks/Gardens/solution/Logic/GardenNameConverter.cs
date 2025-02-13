using System.Numerics;

namespace Gardens.Logic;

// ⚠️ Note that the core idea of this solution is to turn garden names into x/y coordinates.
// The coordinates are also used as the file name for the gardens' member assignments.

/// <summary>
/// Converts between garden names (e.g. "Radish Road 1") and coordinate positions in the garden grid.
/// </summary>
public class GardenNameConverter
{
    /// <summary>
    /// The horizontal paths in the garden grid (from specification)
    /// </summary>
    private readonly List<string> horizontalPaths = [
        "Radish Road", "Spinach Spur", "Pumpkin Parkway", "Garlic Grove", "Onion Alley",
        "Chickpea Crossing", "Pepper Promenade", "Beetroot Boulevard",
    ];

    /// <summary>
    /// The vertical paths in the garden grid (from specification)
    /// </summary>
    private readonly List<string> verticalPaths = [
        "Artichoke Alley", "Lentil Lane", "Mushroom Meadow", "Tomato Trail", "Basil Boulevard",
    ];

    /// <summary>
    /// Converts a garden name (e.g. "Radish Road 1") to its corresponding coordinates in the garden grid.
    /// </summary>
    /// <param name="gardenName">The garden name to convert, consisting of a path name and a number</param>
    /// <returns>The corresponding coordinates if the garden name is valid, null otherwise</returns>
    /// <remarks>Garden layout see specification</remarks>
    public GardenCoordinates? ConvertGardenNameToCoordinates(string gardenName)
    {
        // Check if garden name is a horizontal one
        var horizontalIndex = horizontalPaths.FindIndex(path => gardenName.StartsWith(path));
        int verticalIndex = -1;
        string number = "";
        if (horizontalIndex == -1)
        {
            // Check if garden name is a vertical one
            verticalIndex = verticalPaths.FindIndex(path => gardenName.StartsWith(path));
            if (verticalIndex == -1)
            {
                // Neither horizontal nor vertical -> path not found
                return null;
            }

            // Get the substring after the vertical path
            number = gardenName[verticalPaths[verticalIndex].Length..];
        }
        else
        {
            // Get the substring after the horizontal path
            number = gardenName[horizontalPaths[horizontalIndex].Length..];
        }

        // Check if number is valid; path and number are separated by one or more whitespace characters
        if (int.TryParse(number.TrimStart(), out var numberValue)
            && numberValue >= 1
            && ((horizontalIndex != -1 && numberValue <= 5) || (verticalIndex != -1 && numberValue <= 8)))
        {
            if (horizontalIndex != -1)
            {
                // Number value is x, path is y
                return new GardenCoordinates(numberValue, horizontalIndex + 1);
            }
            else
            {
                // Number value is y, path is x
                return new GardenCoordinates(verticalIndex + 1, numberValue);
            }
        }

        return null;
    }
}

public record struct GardenCoordinates(int X, int Y)
    : IAdditionOperators<GardenCoordinates, (int dx, int dy), GardenCoordinates>
{
    public readonly override string ToString() => $"{X}-{Y}";

    // Note how we use IAdditionOperators to make it easier to add a delta to the coordinates.
    // This functionality comes in handy for the neighbor notification. If you are not familiar
    // with IAdditionOperators, do a little research.
    public static GardenCoordinates operator +(GardenCoordinates left, (int dx, int dy) right)
        => new(left.X + right.dx, left.Y + right.dy);
}
