Console.Write("Enter ISBN to check: ");
var isbnToCheck = Console.ReadLine()!;

if (isbnToCheck.Length != 13)
{
    Console.WriteLine("ISBN must be 13 characters long");
    return;
}

foreach(var digit in isbnToCheck)
{
    if (!char.IsDigit(digit))
    {
        Console.WriteLine("ISBN must contain only digits");
        return;
    }
}

// The checksum is the sum of the digits, each multiplied by 1 or 3, alternating.
// Start with 1, then 3, then 1, then 3, etc.
var checksumBase = 0;
for (var i = 0; i < 12; i++)
{
    checksumBase += (isbnToCheck[i] - '0') * (i % 2 == 0 ? 1 : 3);
}

// Get the remainder of the checksum base when divided by 10.
var remainder = checksumBase % 10;

// The checksum is the difference between 10 and the remainder (i.e. a value between 1 and 9)
var checksum = 10 - remainder;

// The checksum must be a single digit -> if it's 10, use 0
if (checksum == 10)
{
    checksum = 0;
}

// Check if the checksum is correct
if (checksum == isbnToCheck[12] - '0')
{
    Console.WriteLine("Checksum is correct");
}
else
{
    Console.WriteLine("Checksum is incorrect");
}

/*
Examples:
The Lord of the Rings: 9780261102385
Harry Potter: 9783551557483
Frau Komachi empfiehlt ein Buch: 9783499010330
*/