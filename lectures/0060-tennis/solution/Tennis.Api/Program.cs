using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
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
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var apiApp = app.MapGroup("/api");
apiApp.MapPost("/games", GamesApi.CreateGamesHandler)
    .Produces<CreatedIdResult>(StatusCodes.Status201Created);
apiApp.MapPost("/games/{gameId}/points", GamesApi.ReportPointHandler);
apiApp.MapGet("/games/{gameId}/score", GamesApi.GetGameScoreHandler).Produces<ScoreResult>();

apiApp.MapGet("/statistics/player/{playerName}", StatisticsApi.GetPlayerStatisticsHandler).Produces<PlayerStatisticsResult>();
apiApp.MapGet("/statistics/tournament/{tournamentName}", StatisticsApi.GetTournamentStatisticsHandler).Produces<TournamentStatisticsResult>();

app.Run();
