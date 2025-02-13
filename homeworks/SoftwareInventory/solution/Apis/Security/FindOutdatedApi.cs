using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoftwareInventoryApi.DataAccess;
using SoftwareInventoryApi.Logic;

namespace SoftwareInventoryApi.Apis.Security;

public static class FindOutdatedApi
{
    public static async Task<Ok<List<Computer>>> HandleFindInstallation([FromBody] FindInstallationRequest request, VersionChecker checker, IJsonFileRepository repository)
    {
        var results = new List<Computer>();
        foreach (var computerId in repository.EnumerateAll())
        {
            var stream = await repository.Open(computerId.Id, false);
            if (stream == null) { continue; }

            var computer = await repository.Get<Computer>(stream);

            var matchingSoftware = computer.Software.FirstOrDefault(s => s.Identifier == request.SoftwareIdentifier);
            if (matchingSoftware != null && (request.VersionFilter == null || checker.IsWithinRange(matchingSoftware.Version, request.VersionFilter)))
            {
                results.Add(computer);
            }
        }

        return TypedResults.Ok(results);

    }

    #region DTOs
    public class FindInstallationRequest
    {
        public required string SoftwareIdentifier { get; set; }
        public string? VersionFilter { get; set; }
    }
    #endregion

    #region Validation
    public static Dictionary<string, string[]> ValidateFindInstallationRequest(FindInstallationRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.SoftwareIdentifier))
        {
            errors[nameof(request.SoftwareIdentifier)] = ["Software identifier is required"];
        }

        if (!string.IsNullOrWhiteSpace(request.VersionFilter) && !ValidationHelpers.VersionFilterRegex().IsMatch(request.VersionFilter))
        {
            errors[nameof(request.VersionFilter)] = ["Version filter must be a valid version number (major/minor/patch) with optional ^ or ~ prefix"];
        }

        return errors;
    }
    #endregion
}
