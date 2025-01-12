using Microsoft.AspNetCore.Mvc;
using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.SoftwareManagement;

public static class AddSoftwareApi
{
    public static async Task<IResult> HandleAddSoftware([FromBody] AddSoftwareDto software, [FromRoute] string computerMacAddress, IJsonFileRepository repository)
    {
        var macWithoutSeparators = computerMacAddress.Replace(":", "");

        await using var stream = await repository.Open(macWithoutSeparators, forWriting: true);
        if (stream == null)
        {
            return Results.Problem($"Computer with MAC address {computerMacAddress} not found", statusCode: StatusCodes.Status404NotFound);
        }

        var computer = await repository.Get<Computer>(stream);

        var existingSoftware = computer.Software.FirstOrDefault(s => s.Identifier == software.Identifier);
        if (existingSoftware != null)
        {
            existingSoftware.LastReported = DateTimeOffset.UtcNow;
            existingSoftware.Name = software.Name;
            existingSoftware.Vendor = software.Vendor;
            existingSoftware.Version = software.Version;
        }
        else
        {
            computer.Software.Add(new Software
            {
                Identifier = software.Identifier,
                Name = software.Name,
                Vendor = software.Vendor,
                Version = software.Version,
                FirstReported = DateTimeOffset.UtcNow
            });
        }

        await repository.Update(stream, computer);
        return Results.Ok(computer);
    }

    #region DTOs
    public class AddSoftwareDto
    {
        public required string Identifier { get; set; }
        public required string Name { get; set; }
        public string? Vendor { get; set; }
        public required string Version { get; set; }
    }
    #endregion

    #region Validation
    public static Dictionary<string, string[]> ValidateAddSoftwareDto(AddSoftwareDto software)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(software.Identifier))
        {
            errors[nameof(software.Identifier)] = ["Identifier is required"];
        }

        if (string.IsNullOrWhiteSpace(software.Name))
        {
            errors[nameof(software.Name)] = ["Name is required"];
        }

        if (!ValidationHelpers.VersionNumberRegex().IsMatch(software.Version))
        {
            errors[nameof(software.Version)] = ["Version must be in format X.X.X"];
        }

        return errors;
    }
    #endregion
} 