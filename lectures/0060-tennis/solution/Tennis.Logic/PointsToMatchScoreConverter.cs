using System.Text.Json.Serialization;

namespace Tennis.Logic;

/// <summary>
/// Converts a list of points into a match score.
/// </summary>
public class PointsToMatchScoreConverter
{
    //public GameScore ConvertPointsToMatchScore(IEnumerable<int> scoringPlayers)
    public GameScore ConvertPointsToMatchScore(GameScore previousGameScore, int scoringPlayer, int bestOfSets /* 3 or 5 */)
    {
        // Create a copy of the previous game score
        var newGameScore = previousGameScore with 
        {
            Sets = [.. previousGameScore.Sets],
            CurrentGameScore = previousGameScore.CurrentGameScore with { }
        };
        
        // Update current game score
        if (newGameScore.CurrentGameScore.Player1Points == 40 && newGameScore.CurrentGameScore.Player2Points == 40)
        {
            // Deuce situation
            if (newGameScore.CurrentGameScore.AdvantagePlayer == null)
            {
                // From deuce to advantage
                newGameScore = newGameScore with 
                { 
                    CurrentGameScore = newGameScore.CurrentGameScore with 
                    { 
                        AdvantagePlayer = scoringPlayer 
                    } 
                };
            }
            else if (newGameScore.CurrentGameScore.AdvantagePlayer == scoringPlayer)
            {
                // Player with advantage scores again, wins the game
                AddGameToSet(newGameScore, scoringPlayer);
                
                // Reset current game score
                newGameScore = newGameScore with 
                { 
                    CurrentGameScore = new ScoreCurrentGame(0, 0, null) 
                };
            }
            else
            {
                // Player without advantage scores, back to deuce
                newGameScore = newGameScore with 
                { 
                    CurrentGameScore = newGameScore.CurrentGameScore with 
                    { 
                        AdvantagePlayer = null 
                    } 
                };
            }
        }
        else
        {
            // Normal scoring (not in deuce)
            if (scoringPlayer == 1)
            {
                if (newGameScore.CurrentGameScore.Player1Points == 0)
                {
                    newGameScore = newGameScore with { CurrentGameScore = newGameScore.CurrentGameScore with { Player1Points = 15 } };
                }
                else if (newGameScore.CurrentGameScore.Player1Points == 15)
                {
                    newGameScore = newGameScore with { CurrentGameScore = newGameScore.CurrentGameScore with { Player1Points = 30 } };
                }
                else if (newGameScore.CurrentGameScore.Player1Points == 30)
                {
                    newGameScore = newGameScore with { CurrentGameScore = newGameScore.CurrentGameScore with { Player1Points = 40 } };
                }
                else if (newGameScore.CurrentGameScore.Player1Points == 40)
                {
                    // Player 1 wins the game
                    AddGameToSet(newGameScore, 1);
                    
                    // Reset current game score
                    newGameScore = newGameScore with { CurrentGameScore = new ScoreCurrentGame(0, 0, null) };
                }
            }
            else if (scoringPlayer == 2)
            {
                if (newGameScore.CurrentGameScore.Player2Points == 0)
                {
                    newGameScore = newGameScore with { CurrentGameScore = newGameScore.CurrentGameScore with { Player2Points = 15 } };
                }
                else if (newGameScore.CurrentGameScore.Player2Points == 15)
                {
                    newGameScore = newGameScore with { CurrentGameScore = newGameScore.CurrentGameScore with { Player2Points = 30 } };
                }   
                else if (newGameScore.CurrentGameScore.Player2Points == 30)
                {
                    newGameScore = newGameScore with { CurrentGameScore = newGameScore.CurrentGameScore with { Player2Points = 40 } };
                }
                else if (newGameScore.CurrentGameScore.Player2Points == 40)
                {
                    // Player 2 wins the game
                    AddGameToSet(newGameScore, 2);
                    
                    // Reset current game score
                    newGameScore = newGameScore with { CurrentGameScore = new ScoreCurrentGame(0, 0, null) };
                }
            }
        }
        
        // Check if the match is over
        if (newGameScore.WinningPlayer == null)
        {
            int setsToWin = bestOfSets / 2 + 1;
            
            // Only count completed sets
            var completedSets = newGameScore.Sets
                .Where((s, i) => i != newGameScore.Sets.Count - 1 || IsSetComplete(s))
                .ToList();
            
            int player1Sets = completedSets.Count(s => s.Player1Games > s.Player2Games);
            int player2Sets = completedSets.Count(s => s.Player2Games > s.Player1Games);
            
            if (player1Sets >= setsToWin)
            {
                newGameScore = newGameScore with { WinningPlayer = 1 };
            }
            else if (player2Sets >= setsToWin)
            {
                newGameScore = newGameScore with { WinningPlayer = 2 };
            }
        }
        
        return newGameScore;
    }
    
    private void AddGameToSet(GameScore gameScore, int scoringPlayer)
    {
        // Get the current set or create a new one if needed
        SetScore currentSet;
        if (gameScore.Sets.Count == 0 || IsSetComplete(gameScore.Sets.Last()))
        {
            currentSet = new SetScore(0, 0);
            gameScore.Sets.Add(currentSet);
        }
        else
        {
            currentSet = gameScore.Sets.Last();
        }
        
        // Update the current set
        if (scoringPlayer == 1)
        {
            currentSet = currentSet with { Player1Games = currentSet.Player1Games + 1 };
        }
        else if (scoringPlayer == 2)
        {
            currentSet = currentSet with { Player2Games = currentSet.Player2Games + 1 };
        }
        
        // Replace the last set with the updated one
        gameScore.Sets[^1] = currentSet;
    }
    
    private bool IsSetComplete(SetScore set)
    {
        // A player needs at least 6 games with a 2-game advantage
        if ((set.Player1Games >= 6 || set.Player2Games >= 6) && 
            Math.Abs(set.Player1Games - set.Player2Games) >= 2)
        {
            return true;
        }
        
        // Handle tiebreak scenario (7-6)
        if ((set.Player1Games == 7 && set.Player2Games == 6) || 
            (set.Player1Games == 6 && set.Player2Games == 7))
        {
            return true;
        }
        
        return false;
    }
}

public record GameScore(
    string Player1Name,
    string Player2Name,
    List<SetScore> Sets,
    [property: JsonPropertyName("currentGame")]
    ScoreCurrentGame CurrentGameScore,
    int? WinningPlayer
);
public record SetScore(
    int Player1Games, 
    int Player2Games);
public record ScoreCurrentGame(
    int Player1Points, 
    int Player2Points,
    [property: JsonPropertyName("advantage")]
    int? AdvantagePlayer);
