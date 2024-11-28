/*
 * Resistor Color Code Calculator
 * 
 * This program calculates the resistance value and tolerance of a resistor based on its color bands.
 * It supports both 4-band and 5-band resistors using the standard electronic color code:
 * 
 * For 4-band resistors:
 * - Band 1: First significant digit
 * - Band 2: Second significant digit
 * - Band 3: Multiplier (power of 10)
 * - Band 4: Tolerance
 * 
 * For 5-band resistors:
 * - Band 1: First significant digit
 * - Band 2: Second significant digit
 * - Band 3: Third significant digit
 * - Band 4: Multiplier (power of 10)
 * - Band 5: Tolerance
 * 
 * The program converts the colors to their corresponding values and calculates:
 * Resistance = Significant digits × Multiplier
 * Final value = Resistance ± Tolerance%
 */

List<string> digitColors = ["Black", "Brown", "Red", "Orange", "Yellow", "Green", "Blue", "Violet", "Gray", "White"];
List<string> multiplierColors = ["Black", "Brown", "Red", "Orange", "Yellow", "Green", "Blue", "Violet", "Gray", "White", "Gold", "Silver"];
List<double> multiplierValues = [1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000, 0.1, 0.01];
List<string> toleranceColors = ["Brown", "Red", "Green", "Blue", "Violet", "Gray", "Gold", "Silver", "None"];
List<double> toleranceValues = [1, 2, 0.5, 0.25, 0.1, 0.05, 5, 10, 20];

Console.WriteLine("Enter the number of color bands (4 or 5): ");
var numBands = int.Parse(Console.ReadLine()!);

Console.WriteLine("Enter the color for the first band: ");
var band1 = Console.ReadLine()!;
Console.WriteLine("Enter the color for the second band: ");
var band2 = Console.ReadLine()!;

string? band3 = null;
if (numBands == 5)
{
    Console.WriteLine("Enter the color for the third band: ");
    band3 = Console.ReadLine()!;
}

Console.WriteLine("Enter the color for the multiplier band: ");
var multiplierBand = Console.ReadLine()!;
Console.WriteLine("Enter the color for the tolerance band: ");
var toleranceBand = Console.ReadLine()!;

var digit1 = digitColors.IndexOf(band1);
var digit2 = digitColors.IndexOf(band2);
var digit3 = band3 != null ? digitColors.IndexOf(band3) : -1;

var significantFigures = digit1.ToString() + digit2.ToString();
if (digit3 != -1)
{
    significantFigures += digit3.ToString();
}

var significantValue = int.Parse(significantFigures);
var multiplier = multiplierValues[multiplierColors.IndexOf(multiplierBand)];
var tolerance = toleranceValues[toleranceColors.IndexOf(toleranceBand)];

var resistance = significantValue * multiplier;

Console.WriteLine($"The resistance value is: {resistance} ohms");
Console.WriteLine($"The tolerance is: ±{tolerance}%");
