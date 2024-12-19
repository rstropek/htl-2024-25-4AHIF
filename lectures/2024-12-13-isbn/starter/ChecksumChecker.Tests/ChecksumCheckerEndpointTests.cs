using System.Net;
using System.Net.Http.Json;

namespace ChecksumChecker.Tests;

public class ChecksumCheckerEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public ChecksumCheckerEndpointTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("9780261102385")]
    [InlineData("9783551557483")]
    [InlineData("9783499010330")]
    public async Task CheckIsbn_ValidIsbn_ReturnsCorrect(string isbn)
    {
        var response = await _httpClient.GetAsync($"/checksum?isbn={isbn}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CheckIsbn_NullInput_ReturnsBadRequest()
    {
        var response = await _httpClient.GetAsync("/checksum");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("123456789012")] // 12 digits
    [InlineData("")] // Empty string
    public async Task CheckIsbn_TooShort_ReturnsTooShort(string isbn)
    {
        var response = await _httpClient.GetAsync($"/checksum?isbn={isbn}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("12345678901234")] // 14 digits
    public async Task CheckIsbn_TooLong_ReturnsTooLong(string isbn)
    {
        var response = await _httpClient.GetAsync($"/checksum?isbn={isbn}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("978026110238A")] // Contains letter
    [InlineData("978026110238.")] // Contains dot
    [InlineData("978-026110238")] // Contains hyphen
    public async Task CheckIsbn_NonDigitCharacters_ReturnsInvalidCharacters(string isbn)
    {
        var response = await _httpClient.GetAsync($"/checksum?isbn={isbn}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("9780261102384")] // Last digit changed
    [InlineData("9780261102386")] // Last digit changed
    public async Task CheckIsbn_InvalidChecksum_ReturnsChecksumIncorrect(string isbn)
    {
        var response = await _httpClient.GetAsync($"/checksum?isbn={isbn}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("9780261102385")] // Valid ISBN without missing digit
    public async Task GetPossibleIsbns_ValidIsbnNoMissingDigit_ReturnsSingleIsbn(string isbn)
    {
        var response = await _httpClient.GetAsync($"/possible-isbns?isbn={isbn}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<List<string>>();
        Assert.NotNull(content);
        Assert.Single(content);
        Assert.Equal(isbn, content![0]);
    }

    [Theory]
    [InlineData("978026110238.")] // Missing last digit
    public async Task GetPossibleIsbns_MissingLastDigit_ReturnsValidOptions(string isbn)
    {
        var response = await _httpClient.GetAsync($"/possible-isbns?isbn={isbn}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadFromJsonAsync<List<string>>();
        Assert.NotNull(content);
        Assert.Contains("9780261102385", content);
        Assert.DoesNotContain("9780261102384", content);
    }

    [Theory]
    [InlineData("9780261102384")] // Invalid checksum
    public async Task GetPossibleIsbns_InvalidIsbnNoMissingDigit_ReturnsNotFound(string isbn)
    {
        var response = await _httpClient.GetAsync($"/possible-isbns?isbn={isbn}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("978026110238.5")] // Too long with missing digit
    [InlineData("97802611023")] // Too short with missing digit
    public async Task GetPossibleIsbns_InvalidLength_ReturnsNotFound(string isbn)
    {
        var response = await _httpClient.GetAsync($"/possible-isbns?isbn={isbn}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CheckIsbn_BatchValidation_ReturnsCorrectResults()
    {
        var isbns = new List<string>
        {
            "9780261102385", // valid
            "9780261102384", // invalid checksum
            "978026110238A", // invalid characters
            "123456789012"   // too short
        };

        var response = await _httpClient.PostAsJsonAsync("/checksum", isbns);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var results = await response.Content.ReadFromJsonAsync<List<IsbnChecksumResultDto>>();
        Assert.Equal(4, results!.Count);
        Assert.True(results[0].IsValid);
        Assert.False(results[1].IsValid);
        Assert.False(results[2].IsValid);
        Assert.False(results[3].IsValid);
    }
}