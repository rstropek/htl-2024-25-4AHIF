using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.SoftwareManagement;

public static class AddSoftwareApi
{
    public static async Task<IResult> HandleAddSoftware(AddSoftwareDto software, string computerMacAddress, IJsonFileRepository repository)
    {
        var id = computerMacAddress.Replace(":", "");

        await using var stream = await repository.Open(id, forWriting: true);
        if (stream == null)
        {
            return Results.Problem($"Computer with MAC address {computerMacAddress} not found", statusCode: StatusCodes.Status404NotFound);
        }

        var computer = await repository.Get<Computer>(stream) ?? throw new InvalidOperationException("Failed to read existing computer data");

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
    public static async ValueTask<object?> ValidateAddSoftwareDto(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var software = context.GetArgument<AddSoftwareDto>(0);
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(software.Identifier))
        {
            errors["Identifier"] = ["Identifier is required"];
        }

        if (string.IsNullOrWhiteSpace(software.Name))
        {
            errors["Name"] = ["Name is required"];
        }

        var versionSegments = software.Version.Split('.');
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