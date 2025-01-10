using SoftwareInventoryApi.DataAccess;
using SoftwareInventoryApi.Logic;

namespace SoftwareInventoryApi.Apis.Security;

public static class FindOutdatedApi
{
    public static async Task<IResult> HandleFindOutdated(FindOutdatedRequest request, VersionChecker checker, IJsonFileRepository repository)
    {
        var results = new List<Computer>();
        foreach (var computerId in repository.EnumerateAll())
        {
            var stream = await repository.Open(computerId.Id, false);
            if (stream == null) { continue;}

            var computer = await repository.Get<Computer>(stream) ?? throw new InvalidOperationException("Failed to read computer");

            var matchingSoftware = computer.Software.FirstOrDefault(s => s.Identifier == request.SoftwareIdentifier);
            if (matchingSoftware != null && checker.IsOutdated(request.Version, matchingSoftware.Version))
            {
                results.Add(computer);
            }
        }

        return Results.Ok(results);
    }

    #region DTOs
    public class FindOutdatedRequest
    {
        public required string SoftwareIdentifier { get; set; }
        public required string Version { get; set; }
    }
    #endregion

    #region Validation
    public static async ValueTask<object?> ValidateFindOutdatedRequest(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.GetArgument<FindOutdatedRequest>(0);
        var errors = new Dictionary<string, string[]>();

        // Software Identifier validation
        if (string.IsNullOrWhiteSpace(request.SoftwareIdentifier))
        {
            errors["SoftwareIdentifier"] = ["Software identifier is required"];
        }

        var versionSegments = request.Version.Split('.');
        if (versionSegments.Length != 3 || versionSegments.Any(segment => !int.TryParse(segment, out _)))
        {
            errors["Version"] = ["Version must be in format X.X.X"];
        }

        if (errors.Any())
        {
            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
    #endregion
}
