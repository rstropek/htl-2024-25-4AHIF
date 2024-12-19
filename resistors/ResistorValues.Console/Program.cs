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

Dictionary<string, double> multipliers = new()
{
    ["Black"] = 1,
    ["Brown"] = 10,
    ["Red"] = 100,
    ["Orange"] = 1000,
    ["Yellow"] = 10000,
    ["Green"] = 100000,
    ["Blue"] = 1000000,
    ["Violet"] = 10000000,
    ["Gray"] = 100000000,
    ["White"] = 1000000000,
    ["Gold"] = 0.1,
    ["Silver"] = 0.01
};

Dictionary<string, double> tolerances = new()
{
    ["Brown"] = 1,
    ["Red"] = 2,
    ["Green"] = 0.5,
    ["Blue"] = 0.25,
    ["Violet"] = 0.1,
    ["Gray"] = 0.05,
    ["Gold"] = 5,
    ["Silver"] = 10,
    ["None"] = 20
};

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

var digit1 = multipliers[band1];
var digit2 = multipliers[band2];
var digit3 = band3 != null ? multipliers[band3] : -1;

var significantFigures = digit1.ToString() + digit2.ToString();
if (digit3 != -1)
{
    significantFigures += digit3.ToString();
}

var significantValue = int.Parse(significantFigures);
var multiplier = multipliers[multiplierBand];
var tolerance = tolerances[toleranceBand];

var resistance = significantValue * multiplier;

Console.WriteLine($"The resistance value is: {resistance} ohms");
Console.WriteLine($"The tolerance is: ±{tolerance}%");
