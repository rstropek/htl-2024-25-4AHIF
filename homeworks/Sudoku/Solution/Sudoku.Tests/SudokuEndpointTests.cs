using System.Net;
using System.Net.Http.Json;
using IntegrationTests.Helpers;
using Sudoku;

public class SudokuEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public SudokuEndpointTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Validate()
    {
        var response = await _httpClient.PostAsJsonAsync("/validate", new byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,2, 6,4,5
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ValidateResponse>();

        Assert.NotNull(result);
        Assert.True(result.Valid);
        Assert.True(result.Solved);
    }

    [Fact]
    public async Task Validate_WithUnsolved_ReturnsValidButNotSolved()
    {
        var board = new byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,null,2, 6,4,5
        };

        var response = await _httpClient.PostAsJsonAsync("/validate", board);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ValidateResponse>();
        Assert.NotNull(result);
        Assert.True(result.Valid);
        Assert.False(result.Solved);
    }

    [Fact]
    public async Task Validate_WithInvalidBoard_ReturnsInvalid()
    {
        var board = new byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            1,7,8, 3,1,2, 6,4,5
        };

        var response = await _httpClient.PostAsJsonAsync("/validate", board);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ValidateResponse>();
        Assert.NotNull(result);
        Assert.False(result.Valid);
    }

    [Fact]
    public async Task Validate_WithInvalidLength_ReturnsBadRequest()
    {
        var response = await _httpClient.PostAsJsonAsync("/validate", new byte?[] { 1 });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Set_WithInvalidMove_ReturnsBadRequest()
    {
        var request = new
        {
            board = new byte?[] {
                1,2,3, 4,5,6, 7,8,9,
                4,5,6, 7,8,9, 1,2,3,
                7,8,9, 1,2,3, 4,5,6,

                2,3,1, 5,6,4, 8,9,7,
                5,6,4, 8,9,7, 2,3,1,
                8,9,7, 2,3,1, 5,6,4,

                3,1,2, 6,4,5, 9,7,8,
                6,4,5, 9,7,8, 3,1,2,
                9,7,8, 3,1,2, 6,4,5
            },
            row = 8,
            column = 4,
            value = 1
        };

        var response = await _httpClient.PostAsJsonAsync("/set", request);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Set_WithValidMove_ReturnsOk()
    {
        var request = new
        {
            board = new byte?[] {
                1,2,3, 4,5,6, 7,8,9,
                4,5,6, 7,8,9, 1,2,3,
                7,8,9, 1,2,3, 4,5,6,

                2,3,1, 5,6,4, 8,9,7,
                5,6,4, 8,9,7, 2,3,1,
                8,9,7, 2,3,1, 5,6,4,

                3,1,2, 6,4,5, 9,7,8,
                6,4,5, 9,7,8, 3,1,2,
                9,7,8, 3,null,2, 6,4,5
            },
            row = 8,
            column = 4,
            value = 1
        };

        var response = await _httpClient.PostAsJsonAsync("/set", request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}