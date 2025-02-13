using System.Text.Json.Serialization;
using Registration.Api.Apis;
using Registration.Api.Apis.Admin;
using Registration.Api.CrossCuttingConcerns;
using Registration.Api.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Create the data directory if it does not exist.
var repositorySettings = builder.Configuration.GetSection("RepositorySettings").Get<RepositorySettings>()
    ?? throw new InvalidOperationException("RepositorySettings are not configured");
var dataPath = repositorySettings.DataFolder;
if (!Directory.Exists(dataPath))
{
    Directory.CreateDirectory(dataPath);
}

// Add the services required for the admin API
builder.Services.AddAdminApi();
builder.Services.AddSingleton<IJsonFileRepository>(_ => new JsonFileRepository(repositorySettings));
builder.Services.Configure<RepositorySettings>(builder.Configuration.GetSection("RepositorySettings"));
builder.Services.Configure<ErrorHandlingOptions>(builder.Configuration.GetSection("ErrorHandling"));
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseErrorHandling(app.Environment.IsDevelopment());

// Add a simple ping endpoint to check if the API is running.
app.MapGet("/ping", () => "pong!");

// Add the web APIs to the application.
app.MapGroup("api")
    .MapAdminApi();

app.Run();
