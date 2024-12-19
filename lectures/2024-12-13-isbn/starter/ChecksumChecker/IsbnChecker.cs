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

        // TODO: Implement the checksum calculation logic.
        throw new NotImplementedException();
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
        
        // NOTE: ChecksumChecker.Console/Program.cs contains the checksum calculation logic.

        // TODO: Implement the logic to find all valid ISBN numbers that can be formed by replacing a single missing digit (represented by '.') in the input.
        throw new NotImplementedException();
    }
}