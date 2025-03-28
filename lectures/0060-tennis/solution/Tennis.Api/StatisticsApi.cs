using Tennis.DataAccess;

namespace Tennis.Api;

public static class StatisticsApi
{
    public static async Task<IResult> GetPlayerStatisticsHandler(string playerName, ApplicationDataContext context)
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> GetTournamentStatisticsHandler(string tournamentName, ApplicationDataContext context)
    {
        throw new NotImplementedException();
    }

    public record PlayerStatisticsResult(
        string PlayerName,
        int GamesWon,
        int GamesLost,
        float FirstServePercentage,
        float AcePercentage
    );

    public record TournamentStatisticsResult(
        string TournamentName,
        List<PlayerStatisticsResult> Players
    );
}