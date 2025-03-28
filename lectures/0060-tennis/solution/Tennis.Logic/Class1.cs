namespace Tennis.Logic;

/// <summary>
/// Converts a list of points into a match score.
/// </summary>
public class PointsToMatchScoreConverter
{
    public ScoreResult ConvertPointsToMatchScore(IEnumerable<Point> points)
    {
        throw new NotImplementedException();
    }

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
