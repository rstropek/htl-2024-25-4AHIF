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

        // Validate board
        app.MapPost("/validate", (SudokuBoard board, byte?[] sudokuBoard) =>
        {
            return board.Validate(sudokuBoard) switch
            {
                SudokuStatus.ValidUnsolved => Results.Ok(new ValidateResponse(true, false)),
                SudokuStatus.ValidSolved => Results.Ok(new ValidateResponse(true, true)),
                SudokuStatus.Invalid => Results.Ok(new ValidateResponse(false)),
                _ => Results.InternalServerError(),
            };
        });

        // Try set value
        app.MapPost("/set", (SudokuBoard board, SetRequest request) =>
        {
            if (board.TrySet(request.Board, request.Row, request.Column, request.Value))
            {
                return Results.Ok(request.Board);
            }
            else
            {
                return Results.BadRequest();
            }
        });

        app.Run();
    }
}

// Request model for the set endpoint
public record SetRequest(byte?[] Board, byte Row, byte Column, byte Value);

public record ValidateResponse(bool Valid, bool? Solved = null);