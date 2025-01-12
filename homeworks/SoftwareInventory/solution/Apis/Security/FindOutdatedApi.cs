using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SoftwareInventoryApi.DataAccess;
using SoftwareInventoryApi.Logic;

namespace SoftwareInventoryApi.Apis.Security;

public static class FindOutdatedApi
{
    public static async Task<Ok<List<Computer>>> HandleFindOutdated([FromBody] FindOutdatedRequest request, VersionChecker checker, IJsonFileRepository repository)
    {
        var results = new List<Computer>();
        foreach (var computerId in repository.EnumerateAll())
        {
            var stream = await repository.Open(computerId.Id, false);
            if (stream == null) { continue; }

            var computer = await repository.Get<Computer>(stream);

            var matchingSoftware = computer.Software.FirstOrDefault(s => s.Identifier == request.SoftwareIdentifier);
            if (matchingSoftware != null && checker.IsOutdated(request.Version, matchingSoftware.Version))
            {
                results.Add(computer);
            }
        }

        return TypedResults.Ok(results);
    }

    #region DTOs
    public class FindOutdatedRequest
    {
        public required string SoftwareIdentifier { get; set; }
        public required string Version { get; set; }
    }
    #endregion

    #region Validation
    public static Dictionary<string, string[]> ValidateFindOutdatedRequest(FindOutdatedRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        // Software Identifier validation
        if (string.IsNullOrWhiteSpace(request.SoftwareIdentifier))
        {
            errors[nameof(request.SoftwareIdentifier)] = ["Software identifier is required"];
        }

        if (!ValidationHelpers.VersionNumberRegex().IsMatch(request.Version))
        {
            errors[nameof(request.Version)] = ["Version must be in format X.X.X"];
        }

        return errors;
    }
    #endregion
}
