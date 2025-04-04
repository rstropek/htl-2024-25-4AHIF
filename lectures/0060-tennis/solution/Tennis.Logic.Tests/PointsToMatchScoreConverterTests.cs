using System.Text.Json;

namespace Tennis.Logic.Tests;

public class PointsToMatchScoreConverterTests
{
    [Fact]
    public void Normal_Scoring_Progression_Player1()
    {
        // Arrange
        var gameScore = new GameScore(
            Player1Name: "Player 1",
            Player2Name: "Player 2",
            Sets: [],
            CurrentGameScore: new ScoreCurrentGame(0, 0, null),
            WinningPlayer: null
        );
        var converter = new PointsToMatchScoreConverter();

        // Act & Assert - First point (0 -> 15)
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 1, 3);
        Assert.Equal(15, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(0, gameScore.CurrentGameScore.Player2Points);

        // Second point (15 -> 30)
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 1, 3);
        Assert.Equal(30, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(0, gameScore.CurrentGameScore.Player2Points);

        // Third point (30 -> 40)
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 1, 3);
        Assert.Equal(40, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(0, gameScore.CurrentGameScore.Player2Points);

        // Fourth point (40 -> game won)
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 1, 3);
        Assert.Equal(0, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(0, gameScore.CurrentGameScore.Player2Points);
        Assert.Single(gameScore.Sets);
        Assert.Equal(1, gameScore.Sets[0].Player1Games);
        Assert.Equal(0, gameScore.Sets[0].Player2Games);
    }

    [Fact]
    public void Normal_Scoring_Progression_Player2()
    {
        // Arrange
        var gameScore = new GameScore(
            Player1Name: "Player 1",
            Player2Name: "Player 2",
            Sets: [],
            CurrentGameScore: new ScoreCurrentGame(0, 0, null),
            WinningPlayer: null
        );
        var converter = new PointsToMatchScoreConverter();

        // Act & Assert
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 2, 3);
        Assert.Equal(0, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(15, gameScore.CurrentGameScore.Player2Points);

        gameScore = converter.ConvertPointsToMatchScore(gameScore, 2, 3);
        Assert.Equal(0, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(30, gameScore.CurrentGameScore.Player2Points);

        gameScore = converter.ConvertPointsToMatchScore(gameScore, 2, 3);
        Assert.Equal(0, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(40, gameScore.CurrentGameScore.Player2Points);

        gameScore = converter.ConvertPointsToMatchScore(gameScore, 2, 3);
        Assert.Equal(0, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(0, gameScore.CurrentGameScore.Player2Points);
        Assert.Single(gameScore.Sets);
        Assert.Equal(0, gameScore.Sets[0].Player1Games);
        Assert.Equal(1, gameScore.Sets[0].Player2Games);
    }

    [Fact]
    public void Deuce_Scenario()
    {
        // Arrange
        var gameScore = new GameScore(
            Player1Name: "Player 1",
            Player2Name: "Player 2",
            Sets: [],
            CurrentGameScore: new ScoreCurrentGame(40, 40, null),
            WinningPlayer: null
        );
        var converter = new PointsToMatchScoreConverter();

        // Act & Assert - Player 1 gets advantage
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 1, 3);
        Assert.Equal(40, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(40, gameScore.CurrentGameScore.Player2Points);
        Assert.Equal(1, gameScore.CurrentGameScore.AdvantagePlayer);

        // Player 2 scores, back to deuce
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 2, 3);
        Assert.Equal(40, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(40, gameScore.CurrentGameScore.Player2Points);
        Assert.Null(gameScore.CurrentGameScore.AdvantagePlayer);

        // Player 2 gets advantage
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 2, 3);
        Assert.Equal(40, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(40, gameScore.CurrentGameScore.Player2Points);
        Assert.Equal(2, gameScore.CurrentGameScore.AdvantagePlayer);

        // Player 2 wins the game
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 2, 3);
        Assert.Equal(0, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(0, gameScore.CurrentGameScore.Player2Points);
        Assert.Null(gameScore.CurrentGameScore.AdvantagePlayer);
        Assert.Single(gameScore.Sets);
        Assert.Equal(0, gameScore.Sets[0].Player1Games);
        Assert.Equal(1, gameScore.Sets[0].Player2Games);
    }

    [Fact]
    public void Complete_Set()
    {
        // Arrange
        var gameScore = new GameScore(
            Player1Name: "Player 1",
            Player2Name: "Player 2",
            Sets: [new SetScore(5, 0)],
            CurrentGameScore: new ScoreCurrentGame(40, 0, null),
            WinningPlayer: null
        );
        var converter = new PointsToMatchScoreConverter();

        // Act - Player 1 scores to win the set 6-0
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 1, 3);

        // Assert
        Assert.Single(gameScore.Sets);
        Assert.Equal(6, gameScore.Sets[0].Player1Games);
        Assert.Equal(0, gameScore.Sets[0].Player2Games);
        Assert.Equal(0, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(0, gameScore.CurrentGameScore.Player2Points);
    }

    [Fact]
    public void Complete_Match_Best_Of_3()
    {
        // Arrange
        var gameScore = new GameScore(
            Player1Name: "Player 1",
            Player2Name: "Player 2",
            Sets: [
                new SetScore(6, 4),
                new SetScore(5, 0)
            ],
            CurrentGameScore: new ScoreCurrentGame(40, 0, null),
            WinningPlayer: null
        );
        var converter = new PointsToMatchScoreConverter();

        // Act - Player 1 scores to win second set and match
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 1, 3);

        // Assert
        Assert.Equal(2, gameScore.Sets.Count);
        Assert.Equal(6, gameScore.Sets[1].Player1Games);
        Assert.Equal(0, gameScore.Sets[1].Player2Games);
        Assert.Equal(1, gameScore.WinningPlayer);
    }

    [Fact]
    public void Complete_Match_Best_Of_5()
    {
        // Arrange
        var gameScore = new GameScore(
            Player1Name: "Player 1",
            Player2Name: "Player 2",
            Sets: [
                new SetScore(4, 6),  // Player 2 wins first set
                new SetScore(3, 6),  // Player 2 wins second set
                new SetScore(6, 3),  // Player 1 wins third set
                new SetScore(5, 6)   // Current set: Player 2 leading 6-5
            ],
            CurrentGameScore: new ScoreCurrentGame(0, 40, null),
            WinningPlayer: null
        );
        var converter = new PointsToMatchScoreConverter();

        // Act - Player 2 scores to win the game, the fourth set and match
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 2, 5);

        // Assert
        Assert.Equal(4, gameScore.Sets.Count);
        Assert.Equal(5, gameScore.Sets[3].Player1Games);
        Assert.Equal(7, gameScore.Sets[3].Player2Games);
        Assert.Equal(2, gameScore.WinningPlayer); // Player 2 wins the match 3-1
    }

    [Fact]
    public void Tiebreak_Set_Completes()
    {
        // Arrange
        var gameScore = new GameScore(
            Player1Name: "Player 1",
            Player2Name: "Player 2",
            Sets: [new SetScore(6, 6)],
            CurrentGameScore: new ScoreCurrentGame(40, 0, null),
            WinningPlayer: null
        );
        var converter = new PointsToMatchScoreConverter();

        // Act - Player 1 scores to win the tiebreak and the set 7-6
        gameScore = converter.ConvertPointsToMatchScore(gameScore, 1, 3);

        // Assert
        Assert.Single(gameScore.Sets);
        Assert.Equal(7, gameScore.Sets[0].Player1Games);
        Assert.Equal(6, gameScore.Sets[0].Player2Games);
        Assert.Equal(0, gameScore.CurrentGameScore.Player1Points);
        Assert.Equal(0, gameScore.CurrentGameScore.Player2Points);
    }
}
