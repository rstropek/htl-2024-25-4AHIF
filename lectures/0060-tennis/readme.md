# Tennis Match API Specification

## Overview
This exercise requires students to implement a RESTful Web API using ASP.NET Core and Entity Framework Core. The domain is professional Tennis matches. The API will allow for creating and updating matches, recording points and retrieving live scores and player statistics.

## Domain Model
- **Game**
  - `Id` (GUID)
  - `TournamentName` (string)
  - `Player1Name` (string)
  - `Player2Name` (string)
  - `StartDateTime` (DateTime)
  - `EndDateTime` (DateTime?)
  - `BestOfSets` (int: 3 or 5)
  - `Points` (Collection of Point entities)
  - `Winner` (int?, optional: 1 or 2)

- **Point**
  - `Id` (GUID)
  - `GameId` (GUID, foreign key)
  - `ScoringPlayer` (int: 1 or 2)
  - `ServingPlayer` (int: 1 or 2)
  - `Ace` (bool)
  - `ServeType` (enum: FirstServe, SecondServe, DoubleFault)
  - `Out` (bool)
  - `Net` (bool)
  - `Timestamp` (DateTime)

## Tennis Scoring System

In a game, a player needs to win 4 points. However, the points are not counted as 1-2-3-4 but rather as 15-30-40-Game. After winning the first rally, the score is 15:0. If the same player wins the next point, the score becomes 30:0; otherwise, it could be 15:15.

If a player who already has 40 points wins the next point, it's called "Game" and the player wins the game, earning the first point in the set. The set score would then be 1:0 and the next game begins.

A special case occurs when both players have won three rallies and the score is 40:40. This is called "Deuce" in tennis. If a player wins the next rally, it's called "Advantage"—they are in the lead to win the current game. If the same player wins the following point, it's "Game". But if the opponent wins the next rally, the score goes back to Deuce. A player must win two consecutive rallies after Deuce to win the game.

The server always announces the current score. If the server is in advantage, it's called "Advantage In" or "Ad-In". If the receiver is in advantage, it's called "Advantage Out" or "Ad-Out".

A match consists of multiple sets. A set is made up of multiple games. To win a set, a player must win at least six games with a margin of two games. If the score reaches 6:6, a tie-break is played to decide the set.

## API Endpoints

### Create Game
- **POST** `/api/games`
- **Request Body:**
```json
{
  "tournamentName": "Australian Open",
  "player1Name": "Player A",
  "player2Name": "Player B",
  "startDateTime": "2025-01-01T10:00:00Z",
  "bestOfSets": 3
}
```
- **Response:** 201 Created with Game ID

### Update Game
- **PUT** `/api/games/{id}`
- **Request Body:**
```json
{
  "tournamentName": "Updated Name",
  "player1Name": "Player A",
  "player2Name": "Player B",
  "startDateTime": "2025-01-01T10:00:00Z",
  "endDateTime": "2025-01-01T12:30:00Z",
  "bestOfSets": 3
}
```
- **Response:** 204 No Content

### Report Point
- **POST** `/api/games/{gameId}/points`
- **Request Body:**
```json
{
  "scoringPlayer": 1,
  "servingPlayer": 1,
  "ace": true,
  "serveType": "FirstServe",
  "out": false,
  "net": false,
  "timestamp": "2025-01-01T10:15:00Z"
}
```
- **Response:** 201 Created

### Get Current Score
- **GET** `/api/games/{gameId}/score`
- **Response:**
```json
{
  "player1": "Player A",
  "player2": "Player B",
  "sets": [
    { "player1Games": 4, "player2Games": 3 },
    { "player1Games": 2, "player2Games": 1 }
  ],
  "currentGame": {
    "player1Points": "40",
    "player2Points": "30"
  },
  "winner": null
}
```
Note: The `winner` field is optional and only present if the match has been won.

### Get Player Statistics
- **GET** `/api/statistics/player/{playerName}`
- **Response:**
```json
{
  "playerName": "Player A",
  "gamesWon": 10,
  "gamesLost": 5,
  "firstServePercentage": 0.82,
  "acePercentage": 0.21
}
```

### Get Tournament Statistics
- **GET** `/api/statistics/tournament/{tournamentName}`
- **Response:**
```json
{
  "tournamentName": "Australian Open",
  "players": [
    {
      "playerName": "Player A",
      "gamesWon": 6,
      "gamesLost": 2,
      "firstServePercentage": 0.75,
      "acePercentage": 0.18
    },
    {
      "playerName": "Player B",
      "gamesWon": 2,
      "gamesLost": 6,
      "firstServePercentage": 0.68,
      "acePercentage": 0.15
    }
  ]
}
```

## Notes

There is no need to implement separate tables for players or tournaments; instead, player and tournament names should be stored directly as strings within the Game entity. 

The logic that handles score computation must fully adhere to official tennis scoring rules as previously described. This logic should be strictly separated from database access to ensure that it is testable in isolation.

Player and tournament statistics must be derived from analyzing the recorded Point data, rather than being stored or maintained independently.

