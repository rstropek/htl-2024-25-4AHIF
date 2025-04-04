# Quidditch World Cup 2027 API Coding Exercise

![hero](./hero.png)

## Overview

In this exercise, you'll build a web API to manage the **Quidditch World Cup 2027** tournament. The tournament features 16 national teams competing in a single-elimination knockout format, where teams are paired in matches and winners advance to the next round until a champion is crowned.

## Tournament Structure

- 16 teams participate in the tournament
- Single-elimination knockout format (teams eliminated after one loss)
- 4 rounds of play: Round of 16, Quarter-finals, Semi-finals, and Finals
- Each match produces exactly one winner who advances to the next round
- The tournament concludes when one team remains undefeated

## Technical Requirements

- Implement a web API using ASP.NET Core Minimal API and Entity Framework Core
- Include appropriate error handling and validation
- Document your code where necessary

## API Endpoints

### Set Participating Teams

**Endpoint:** `POST /api/teams`  
**Description:**  
This endpoint registers the 16 national teams that will participate in the tournament.

**Requirements:**  
- Accepts an array of exactly 16 team names or team objects
- Validates that exactly 16 teams are provided
- Each team must have a unique name
- Returns an error if teams have already been set
- Stores the list for use in the tournament

**Sample Request:**

```json
POST /api/teams
Content-Type: application/json

{
  "teams": [
    "Austria",
    "Belgium",
    "Canada",
    "Denmark",
    "Egypt",
    "Finland",
    "Greece",
    "Hungary",
    "India",
    "Japan",
    ...
  ]
}
```

### Generate Random Tournament Bracket

**Endpoint:** `POST /api/generate-bracket`  
**Description:**  
This endpoint randomly assigns the 16 teams to the initial tournament bracket, creating a complete match hierarchy.

**Requirements:**  
- Teams must have been registered first
- Teams should be randomly paired for the first round
- Each match should have a unique identifier
- The complete bracket structure should be returned and stored for later use
- Should return an error if the bracket has already been generated

**Sample Request:**

```json
POST /api/generate-bracket
Content-Type: application/json
```

### Report Match Result

**Endpoint:** `POST /api/report-match`  
**Description:**  
This endpoint is used to report the result of a completed match, determining which team advances to the next round.

**Requirements:**  
- Accepts a match ID and the points scored by each team
- Validates that the match ID exists in the tournament bracket
- Ensures that the points are non-negative integers
- Ensures that one team scored higher than the other (no ties allowed in Quidditch)
- Ensures that results for the matches leading up to this match have already been reported
- Updates the tournament bracket with the result
- Returns an error if the match result has already been reported

**Sample Request:**

```json
POST /api/report-match
Content-Type: application/json

{
  "matchId": 4711,
  "teamAPoints": 170,
  "teamBPoints": 60
}
```

### Get Tournament Status

**Endpoint:** `GET /api/tournament`  
**Description:**  
This endpoint retrieves the current state of the tournament, including all matches, results, and the progression of teams.

**Requirements:**  
- Returns the complete tournament bracket with all match results
- Includes information on completed and upcoming matches
- Identifies the tournament winner if the final match has been completed

Good luck, and may the best Quidditch team win!
