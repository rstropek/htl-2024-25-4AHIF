using Microsoft.AspNetCore.Diagnostics;

namespace Sudoku;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddProblemDetails();
        builder.Services.AddSingleton<SudokuBoard>();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseExceptionHandler(exceptionHandlerApp
            => exceptionHandlerApp.Run(async context
                =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    switch (exceptionHandlerPathFeature?.Error)
                    {
                        case ArgumentOutOfRangeException ex:
                            await Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest).ExecuteAsync(context);
                            break;

                        case ArgumentException ex:
                            await Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest).ExecuteAsync(context);
                            break;

                        default:
                            await Results.Problem().ExecuteAsync(context);
                            break;
                    };
                }));

        /// <summary>
        /// Validates a Sudoku board to check if it's valid and/or solved.
        /// </summary>
        /// <response code="200">
        /// Returns a ValidateResponse object containing:
        /// - Valid: true if the board is valid, false otherwise
        /// - Solved: true if the board is solved, false if valid but unsolved, null if invalid
        /// </response>
        /// <response code="500">Returned when an unexpected error occurs</response>
        app.MapPost("/validate", (SudokuBoard board, byte?[] sudokuBoard) =>
        {
            throw new NotImplementedException();
        });

        /// <summary>
        /// Attempts to set a value at the specified position on the Sudoku board.
        /// </summary>
        /// <response code="200">Returns the updated board if the value was successfully set</response>
        /// <response code="400">
        /// Returned when:
        /// - The value cannot be placed at the specified position
        /// - The position is out of range
        /// - The value is invalid (must be 1-9)
        /// </response>
        app.MapPost("/set", (SudokuBoard board, SetRequest request) =>
        {
            throw new NotImplementedException();
        });

        app.Run();
    }
}

// Request model for the set endpoint
public record SetRequest(byte?[] Board, byte Row, byte Column, byte Value);

public record ValidateResponse(bool Valid, bool? Solved = null);