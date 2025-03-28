using Microsoft.EntityFrameworkCore;
using Tennis.DataAccess;

namespace Tennis.Api;

public static class GamesApi
{
    public static async Task<IResult> CreateGamesHandler(CreateGameRequest request, ApplicationDataContext context)
    {
        var newGame = new Game
        {
            TournamentName = request.TournamentName,
            Player1Name = request.Player1Name,
            Player2Name = request.Player2Name,
            StartDateTime = request.StartDateTime,
            BestOfSets = request.BestOfSets
        };

        context.Games.Add(newGame);
        await context.SaveChangesAsync();

        return Results.Created((string?)null, new CreatedIdResult(newGame.Id));
    }

    public static async Task<IResult> ReportPointHandler(int gameId, CreatePointRequest request, ApplicationDataContext context)
    {
        var gameExists = await context.Games.AnyAsync(g => g.Id == gameId);
        if (!gameExists)
        {
            return Results.NotFound();
        }

        var newPoint = new Point
        {
            GameId = gameId,
            ScoringPlayer = request.ScoringPlayer,
            ServingPlayer = request.ServingPlayer,
            Ace = request.Ace,
            ServeType = request.ServeType,
            Out = request.Out,
            Net = request.Net,
            Timestamp = request.Timestamp
        };

        context.Points.Add(newPoint);
        await context.SaveChangesAsync();

        return Results.Created((string?)null, new CreatedIdResult(newPoint.Id));
    }

    public static async Task<IResult> GetGameScoreHandler(int gameId, ApplicationDataContext context)
    {
        var points = await context.Points
            .Where(p => p.GameId == gameId)
            .ToListAsync();
    }
        
    public record CreateGameRequest(
        string TournamentName,
        string Player1Name,
        string Player2Name,
        DateTimeOffset StartDateTime,
        int BestOfSets
    );

    public record CreatedIdResult(
        int Id
    );

    public record CreatePointRequest(
        int ScoringPlayer,
        int ServingPlayer,
        bool Ace,
        ServeType ServeType,
        bool Out,
        bool Net,
        DateTimeOffset Timestamp
    );
}

