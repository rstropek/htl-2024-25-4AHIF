using System.Text.Json;

namespace ChecksumChecker.Tests;

public class ChecksumCheckerTests
{
    private readonly IsbnChecker _checker = new();

    [Theory]
    [InlineData("9780261102385")]
    [InlineData("9783551557483")]
    [InlineData("9783499010330")]
    public void CheckIsbn_ValidIsbn_ReturnsCorrect(string isbn)
    {
        var result = _checker.CheckIsbn(isbn);
        Assert.Equal(IsbnChecksumResult.Correct, result);
    }

    [Fact]
    public void CheckIsbn_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _checker.CheckIsbn(null!));
    }

    [Theory]
    [InlineData("123456789012")] // 12 digits
    [InlineData("")] // Empty string
    public void CheckIsbn_TooShort_ReturnsTooShort(string isbn)
    {
        var result = _checker.CheckIsbn(isbn);
        Assert.Equal(IsbnChecksumResult.TooShort, result);
    }

    [Theory]
    [InlineData("12345678901234")] // 14 digits
    public void CheckIsbn_TooLong_ReturnsTooLong(string isbn)
    {
        var result = _checker.CheckIsbn(isbn);
        Assert.Equal(IsbnChecksumResult.TooLong, result);
    }

    [Theory]
    [InlineData("978026110238A")] // Contains letter
    [InlineData("978026110238.")] // Contains dot
    [InlineData("978-026110238")] // Contains hyphen
    public void CheckIsbn_NonDigitCharacters_ReturnsInvalidCharacters(string isbn)
    {
        var result = _checker.CheckIsbn(isbn);
        Assert.Equal(IsbnChecksumResult.InvalidCharacters, result);
    }

    [Theory]
    [InlineData("9780261102384")] // Last digit changed
    [InlineData("9780261102386")] // Last digit changed
    public void CheckIsbn_InvalidChecksum_ReturnsChecksumIncorrect(string isbn)
    {
        var result = _checker.CheckIsbn(isbn);
        Assert.Equal(IsbnChecksumResult.ChecksumIncorrect, result);
    }

    [Fact]
    public void GetPossibleIsbns_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _checker.GetPossibleIsbns(null!));
    }

    [Theory]
    [InlineData("9780261102385")] // Valid ISBN without missing digit
    public void GetPossibleIsbns_ValidIsbnNoMissingDigit_ReturnsSingleIsbn(string isbn)
    {
        var result = _checker.GetPossibleIsbns(isbn);
        Assert.Single(result);
        Assert.Equal(isbn, result[0]);
    }

    [Theory]
    [InlineData("978026110238.")] // Missing last digit
    public void GetPossibleIsbns_MissingLastDigit_ReturnsValidOptions(string isbn)
    {
        var result = _checker.GetPossibleIsbns(isbn);
        Console.WriteLine(JsonSerializer.Serialize(result));
        Assert.Contains("9780261102385", result); // Should find the valid checksum
        Assert.DoesNotContain("9780261102384", result); // Should not include invalid checksums
    }

    [Theory]
    [InlineData("9780261102384")] // Invalid checksum
    public void GetPossibleIsbns_InvalidIsbnNoMissingDigit_ReturnsEmptyList(string isbn)
    {
        var result = _checker.GetPossibleIsbns(isbn);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("978026110238.5")] // Too long with missing digit
    [InlineData("97802611023")] // Too short with missing digit
    public void GetPossibleIsbns_InvalidLength_ReturnsEmptyList(string isbn)
    {
        var result = _checker.GetPossibleIsbns(isbn);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("978026110.38.")] // Multiple missing digits
    [InlineData("978026110A38.")] // Invalid character and missing digit
    public void GetPossibleIsbns_InvalidFormat_ReturnsEmptyList(string isbn)
    {
        var result = _checker.GetPossibleIsbns(isbn);
        Assert.Empty(result);
    }
}
