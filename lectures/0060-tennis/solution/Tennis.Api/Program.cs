using Microsoft.EntityFrameworkCore;
using Tennis.Api;
using Tennis.DataAccess;
using static Tennis.Api.GamesApi;
using static Tennis.Api.StatisticsApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/games", GamesApi.CreateGamesHandler);
app.MapPost("/api/games/{gameId}/points", GamesApi.ReportPointHandler);
app.MapGet("/api/games/{gameId}/score", GamesApi.GetGameScoreHandler).Produces<ScoreResult>();

app.MapGet("/api/statistics/player/{playerName}", StatisticsApi.GetPlayerStatisticsHandler).Produces<PlayerStatisticsResult>();
app.MapGet("/api/statistics/tournament/{tournamentName}", StatisticsApi.GetTournamentStatisticsHandler).Produces<TournamentStatisticsResult>();

app.Run();
