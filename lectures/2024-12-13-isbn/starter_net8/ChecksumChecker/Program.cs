using ChecksumChecker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<IsbnChecker>();
        var app = builder.Build();

        app.UseHttpsRedirection();

        /// <summary>
        /// Generates possible valid ISBN numbers based on a partial or invalid ISBN
        /// </summary>
        /// <param name="isbn">The ISBN to analyze</param>
        /// <returns>
        /// 200 OK: List of possible valid ISBNs
        /// 404 Not Found: When no valid ISBNs can be generated
        /// </returns>
        app.MapGet("/possible-isbns", (string isbn, IsbnChecker isbnChecker) =>
        {
            // TODO: Implement the HTTP endpoint logic.
            // Use the methods from IsbnChecker.
            // return `Results.NotFound()` or `Results.Ok(...)` as appropriate.
            throw new NotImplementedException();
        });

        /// <summary>
        /// Validates a single ISBN number's checksum
        /// </summary>
        /// <param name="isbn">The ISBN to validate</param>
        /// <returns>
        /// 200 OK: When ISBN is valid, returns success message
        /// 400 Bad Request: When ISBN is invalid, returns specific error message
        /// </returns>
        app.MapGet("/checksum", (string isbn, IsbnChecker isbnChecker) =>
        {
            // TODO: Implement the HTTP endpoint logic.
            // Use the methods from IsbnChecker.
            // return `Results.BadRequest(...)` or `Results.Ok(...)` as appropriate.
            throw new NotImplementedException();
        });

        /// <summary>
        /// Batch validates multiple ISBN numbers
        /// </summary>
        /// <param name="isbns">List of ISBNs to validate</param>
        /// <returns>
        /// 200 OK: Returns list of validation results for each ISBN, containing:
        /// - Original ISBN
        /// - IsValid flag
        /// - Detailed result message
        /// </returns>
        app.MapPost("/checksum", (List<string> isbns, IsbnChecker isbnChecker) =>
        {
            // TODO: Implement the HTTP endpoint logic.
            throw new NotImplementedException();
        });

        app.Run();
    }
}
public record IsbnChecksumResultDto(string Isbn, bool IsValid, string Result);