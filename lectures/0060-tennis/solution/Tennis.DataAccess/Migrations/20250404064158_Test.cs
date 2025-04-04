using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tennis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId1",
                table: "CurrentGameScores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrentGameScores_GameId1",
                table: "CurrentGameScores",
                column: "GameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentGameScores_Games_GameId1",
                table: "CurrentGameScores",
                column: "GameId1",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentGameScores_Games_GameId1",
                table: "CurrentGameScores");

            migrationBuilder.DropIndex(
                name: "IX_CurrentGameScores_GameId1",
                table: "CurrentGameScores");

            migrationBuilder.DropColumn(
                name: "GameId1",
                table: "CurrentGameScores");
        }
    }
}
