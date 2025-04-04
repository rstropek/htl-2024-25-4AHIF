# Quidditch World Cup 2027 API Coding Exercise

## Overview

In this exercise, you'll build an API to manage the **Quidditch World Cup 2027**. The tournament features 16 national teams that will first compete in a knockout tournament. 

## API Endpoints

### Set Participating Teams

**Description:**  
This endpoint is used to set the list of 16 national teams that will participate in the tournament.

**Requirements:**  
- Accepts an array of exactly 16 team names or team objects.
- Validates the number of teams provided.
- Stores the list for use in later phases.

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
    "Kenya",
    "Lithuania",
    ...
  ]
}
```

### Generate Random Game Hierarchy

**Description:**
This endpoint generates a random game hierarchy for the tournament. The hierarchy should be a knockout format, where each match eliminates one team until a winner is determined.

**Requirements:**
- The hierarchy should be generated randomly.
- Each match should have a unique ID.
- The hierarchy should be stored for later use.
- The hierarchy should be returned in a structured format.

**Sample Request:**

```json
POST /api/generate-hierarchy
```

### Report Match Result

**Description:**
This endpoint is used to report the result of a match. The result should include the match ID and the points for each team.

**Requirements:**
- Accepts a match ID and the points for each team.
- Validates the match ID.
- Verifies that the points are not equal or negative.
- Updates the match result in the hierarchy.

**Sample Request:**

```json
POST /api/report-match
Content-Type: application/json

{
  "matchId": "12345",
  "team1Points": 150,
  "team2Points": 120
}
```

### Get Tournament Results

**Description:**
This endpoint retrieves the current state of the tournament, including the match hierarchy and results.

**Requirements:**
- Returns the current state of the tournament.
- Includes the match hierarchy and results.
