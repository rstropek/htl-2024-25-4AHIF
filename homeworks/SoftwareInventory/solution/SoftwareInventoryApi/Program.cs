using System.Text.Json.Serialization;
using SoftwareInventoryApi;
using SoftwareInventoryApi.Apis.ComputerManagement;
using SoftwareInventoryApi.Apis.Security;
using SoftwareInventoryApi.Apis.SoftwareManagement;
using SoftwareInventoryApi.DataAccess;
using SoftwareInventoryApi.Logic;

var builder = WebApplication.CreateBuilder(args);

var repositorySettings = builder.Configuration.GetSection("RepositorySettings").Get<RepositorySettings>() ?? throw new InvalidOperationException("RepositorySettings are not configured");
var dataPath = repositorySettings.DataFolder;
if (!Directory.Exists(dataPath))
{
    Directory.CreateDirectory(dataPath);
}

builder.Services.AddSingleton<IJsonFileRepository>(_ => new JsonFileRepository(repositorySettings));
builder.Services.AddSingleton<VersionChecker>();
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<RouteHandlerOptions>(options =>
{
    options.ThrowOnBadRequest = true;
});

var app = builder.Build();

// Add exception handline (see ./CrossCuttingConcerns/ExceptionHandlingMiddleware.cs)
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapComputerManagementApi()
    .MapSoftwareManagementApi()
    .MapSecurityApi();

app.Run();
