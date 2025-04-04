using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tennis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TournamentName = table.Column<string>(type: "TEXT", nullable: false),
                    Player1Name = table.Column<string>(type: "TEXT", nullable: false),
                    Player2Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartDateTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    EndDateTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    BestOfSets = table.Column<int>(type: "INTEGER", nullable: false),
                    Winner = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrentGameScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameScoreJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentGameScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentGameScores_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScoringPlayer = table.Column<int>(type: "INTEGER", nullable: false),
                    ServingPlayer = table.Column<int>(type: "INTEGER", nullable: false),
                    Ace = table.Column<bool>(type: "INTEGER", nullable: false),
                    ServeType = table.Column<int>(type: "INTEGER", nullable: false),
                    Out = table.Column<bool>(type: "INTEGER", nullable: false),
                    Net = table.Column<bool>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Points_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentGameScores_GameId",
                table: "CurrentGameScores",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_GameId",
                table: "Points",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentGameScores");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
