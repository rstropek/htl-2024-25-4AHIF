using Tennis.DataAccess;

namespace Tennis.Api;

public static class GamesApi
{
    public static async Task<IResult> CreateGamesHandler(CreateGameRequest request, ApplicationDataContext context)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> ReportPointHandler(int gameId, CreatePointRequest request, ApplicationDataContext context)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> GetGameScoreHandler(int gameId, ApplicationDataContext context)
    {
        throw new NotImplementedException();
    }
        
    public record CreateGameRequest(
        string TournamentName,
        string Player1Name,
        string Player2Name,
        DateTimeOffset StartDateTime,
        int BestOfSets
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

    public record ScoreResult(
        string Player1,
        string Player2,
        int? Winner,
        List<ScoreSetResult> Sets,
        ScoreCurrentGameResult CurrentGame
    );

    public record ScoreSetResult(
        int Player1Games,
        int Player2Games
    );

    public record ScoreCurrentGameResult(
        int Player1Points,
        int Player2Points, 
        int? Advantage
    );
}

