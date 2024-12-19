namespace ChecksumChecker;

public enum IsbnChecksumResult
{
    Correct,
    TooShort,
    TooLong,
    InvalidCharacters,
    ChecksumIncorrect,
}

public class IsbnChecker
{
    private IsbnChecksumResult BasicCheck(string isbn, bool allowOneMissingDigit)
    {
        if (isbn.Length < 13)
        {
            return IsbnChecksumResult.TooShort;
        }
        
        if (isbn.Length > 13)
        {
            return IsbnChecksumResult.TooLong;
        }

        var missingDigit = false;
        foreach (var digit in isbn)
        {
            if (digit == '.')
            {
                if (missingDigit || !allowOneMissingDigit)
                {
                    return IsbnChecksumResult.InvalidCharacters;
                }

                missingDigit = true;
            }
            else if (!char.IsDigit(digit))
            {
                return IsbnChecksumResult.InvalidCharacters;
            }
        }

        return IsbnChecksumResult.Correct;
    }

    /// <summary>
    /// Validates a 13-digit ISBN (International Standard Book Number) and verifies its checksum.
    /// </summary>
    /// <param name="isbn">The ISBN string to validate. Must be exactly 13 digits long.</param>
    /// <returns>
    /// An IsbnChecksumResult enum value indicating the validation result:
    /// - Correct: The ISBN is valid and its checksum is correct
    /// - TooShort: The ISBN is less than 13 digits
    /// - TooLong: The ISBN is more than 13 digits
    /// - InvalidCharacters: The ISBN contains non-digit characters
    /// - ChecksumIncorrect: The ISBN's checksum digit is incorrect
    /// </returns>
    public IsbnChecksumResult CheckIsbn(string isbn)
    {
        ArgumentNullException.ThrowIfNull(isbn);

        // NOTE: ChecksumChecker.Console/Program.cs contains the checksum calculation logic.

        var basicCheckResult = BasicCheck(isbn, false);
        if (basicCheckResult != IsbnChecksumResult.Correct)
        {
            return basicCheckResult;
        }

        // Calculate checksum
        var checksumBase = 0;
        for (var i = 0; i < 12; i++)
        {
            checksumBase += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);
        }

        var remainder = checksumBase % 10;
        var checksum = 10 - remainder;
        if (checksum == 10)
        {
            checksum = 0;
        }

        if (checksum == isbn[12] - '0')
        {
            return IsbnChecksumResult.Correct;
        }

        return IsbnChecksumResult.ChecksumIncorrect;
    }

    /// <summary>
    /// Finds all valid ISBN numbers that can be formed by replacing a single missing digit (represented by '.') in the input.
    /// </summary>
    /// <param name="isbn">The ISBN string to process. Must be 13 characters long and can contain one '.' representing a missing digit.</param>
    /// <returns>
    /// A list of valid ISBN numbers:
    /// - If the input contains no '.', returns a list with just the input ISBN if it's valid
    /// - If the input contains one '.', returns a list of all valid ISBNs that can be formed by replacing the '.'
    /// - Returns an empty list if the input is invalid (wrong length, multiple '.' characters, or invalid characters)
    /// </returns>
    public List<string> GetPossibleIsbns(string isbn)
    {
        ArgumentNullException.ThrowIfNull(isbn);
        
        var basicCheckResult = BasicCheck(isbn, true);
        if (basicCheckResult != IsbnChecksumResult.Correct)
        {
            return [];
        }

        var indexOfMissingDigit = isbn.IndexOf('.');
        if (indexOfMissingDigit == -1)
        {
            if (CheckIsbn(isbn) == IsbnChecksumResult.Correct)
            {
                return [isbn];
            }

            return [];
        }

        var before = isbn[..indexOfMissingDigit];
        var after = isbn[(indexOfMissingDigit + 1)..];
        List<string> possibleIsbns = [];

        for (var i = 0; i < 10; i++)
        {
            var possibleIsbn = $"{before}{i}{after}";
            if (CheckIsbn(possibleIsbn) == IsbnChecksumResult.Correct)
            {
                possibleIsbns.Add(possibleIsbn);
            }
        }

        return possibleIsbns;
    }
}