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
            var result = isbnChecker.GetPossibleIsbns(isbn);
            if (result.Count == 0)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);
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
            var result = isbnChecker.CheckIsbn(isbn);
            var resultString = GetResultString(result);
            if (result == IsbnChecksumResult.Correct)
            {
                return Results.Ok(resultString);
            }

            return Results.BadRequest(resultString);
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
            List<IsbnChecksumResultDto> results = [];
            foreach (var isbn in isbns)
            {
                var result = isbnChecker.CheckIsbn(isbn);
                results.Add(new IsbnChecksumResultDto(isbn, result == IsbnChecksumResult.Correct, GetResultString(result)));
            }

            return Results.Ok(results);
        });

        static string GetResultString(IsbnChecksumResult result) => result switch
        {
            IsbnChecksumResult.Correct => "Checksum is correct",
            IsbnChecksumResult.ChecksumIncorrect => "Checksum is incorrect",
            IsbnChecksumResult.TooShort => "ISBN is too short",
            IsbnChecksumResult.TooLong => "ISBN is too long",
            IsbnChecksumResult.InvalidCharacters => "ISBN contains invalid characters",
            _ => "Unknown error",
        };

        app.Run();
    }
}
public record IsbnChecksumResultDto(string Isbn, bool IsValid, string Result);