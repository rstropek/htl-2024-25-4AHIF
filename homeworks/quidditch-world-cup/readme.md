# Quidditch World Cup 2027 API Coding Exercise

## Overview

In this exercise, you'll build an API to manage the **Quidditch World Cup 2027**. The tournament features 32 national teams that will first compete in a group phase and then progress to a knockout phase. Your API will support the following operations:

- **Group Phase:**  
  The 32 teams are randomly divided into 8 groups of 4 teams each. Within each group, every team plays against every other team exactly once (6 matches per group). A win awards a team 1 point, while the loser receives 0 points. The top 2 teams from each group will move on to the knockout stage. In the event of a tie in points, the system randomly selects which team advances. 

- **Knockout Phase:**  
  The knockout phase consists of multiple rounds (Round of 32, Round of 16, Quarterfinals, Semifinals, Third Place Playoff, and Final) where match-ups are generated randomly.

## API Endpoints

### Set Participating Teams

**Description:**  
This endpoint is used to set the list of 32 national teams that will participate in the tournament.

**Requirements:**  
- Accepts an array of exactly 32 team names or team objects.
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

### Randomly Assign Teams into 8 Groups for Group Phase

**Description:**
This endpoint randomly assigns the 32 participating teams into 8 groups of 4 teams each. This action is only allowed after the participating teams have been set.

**Requirements:**
- Checks that the participating teams list is available.
- Randomly divides teams into 8 groups.
- Returns a data structure containing the groups with assigned teams.

**Sample Request:**

```json
POST /api/groups/assign
Content-Type: application/json
```

### Get a List of All Matches in Group Phase

**Description:**
This endpoint returns the schedule of all matches in the group phase. Each match is assigned a unique ID. For matches that have already been played, the results (points for both teams and match time) are included. Additionally, teams in each group should be sorted by their points in descending order.

**Requirements:**
- Ensures that teams have been assigned to groups.
- Provides each match with a unique ID, participating teams, and results (if available).
- Returns groups with teams sorted by points for completed matches.

**Sample Request:**
```json
GET /api/groups/matches
Content-Type: application/json
```

### Set Result of a Match

**Description:**
This endpoint records the result of a specific match.

**Requirements:**
- Accepts a match ID to identify the specific match.
- Requires the points earned by both teams and the total match time until the Snitch is caught.
- Updates the match record with the provided result.

**Sample Request:**
```json
POST /api/matches/result
Content-Type: application/json

{
  "matchId": "...",
  "result": {
    "teamA_points": 1,
    "teamB_points": 0,
    "match_time": 55  // Time in minutes until the Snitch was caught
  }
}
```

### Calculate Next Round

**Description:**
This endpoint calculates and sets up the next round based on the results from the previous phase. For group phase: In the event of a tie, the system randomly selects which team advances.

**Requirements:**
- Validates that all matches have results.
- For group phase:
  - Determines the top 2 teams from each group.
  - Randomly pairs teams to form the Round of 32 fixtures.
- For knockout phase:
  - Generates match-ups for the next round based on the results of the previous round.

**Sample Request:**

json
Copy
POST /api/knockout/round32/calculate
Content-Type: application/json
6. Return State of Knockout Phase
Description:
This endpoint provides the current state of the knockout phase. It includes match results for those games that have been played and displays the scheduled fixtures for upcoming matches.

Requirements:

Returns a summary for each knockout round (Round of 32, Round of 16, Quarterfinals, Semifinals, Third Place Playoff, and Final).

Each match includes a unique ID, participating teams, and results (if available).

Provides a clear overview of the tournament's progress.

Sample Request:

json
Copy
GET /api/knockout/state
Content-Type: application/json
Implementation Guidelines
Validation:
Ensure that input data is valid (e.g., exactly 32 teams are provided, match IDs exist, etc.).

Error Handling:
Handle error cases gracefully, such as when a required step (e.g., setting teams) has not been completed.

Randomization:
Use appropriate randomization methods for both group assignment and tie-breaker scenarios.

Documentation:
Document your code and provide clear instructions for how to test each endpoint.

Good luck and have fun building your Quidditch World Cup 2027 API!