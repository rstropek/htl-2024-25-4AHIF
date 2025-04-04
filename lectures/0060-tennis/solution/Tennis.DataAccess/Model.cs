using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tennis.DataAccess;

public enum ServeType
{
    FirstServe,
    SecondServe,
    DoubleFault
}

public class Game
{
    public int Id { get; set; }
    public string TournamentName { get; set; } = string.Empty;
    public string Player1Name { get; set; } = string.Empty;
    public string Player2Name { get; set; } = string.Empty;
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset? EndDateTime { get; set; }
    public int BestOfSets { get; set; } // 3 or 5
    public List<Point> Points { get; set; } = [];
    public int? Winner { get; set; } // 1 or 2, null if game not finished
}

public class Point
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public Game? Game { get; set; }
    public int ScoringPlayer { get; set; } // 1 or 2
    public int ServingPlayer { get; set; } // 1 or 2
    public bool Ace { get; set; }
    public ServeType ServeType { get; set; }
    public bool Out { get; set; }
    public bool Net { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string? AdditionalInfo { get; set; }
}

public class CurrentGameScore
{
    [Key]
    public int GameId { get; set; }
    public Game? Game { get; set; }
    public string GameScoreJson { get; set; } = string.Empty;
}

