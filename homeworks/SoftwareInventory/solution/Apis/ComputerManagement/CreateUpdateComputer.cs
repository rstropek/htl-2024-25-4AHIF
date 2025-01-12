using Microsoft.AspNetCore.Mvc;
using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.ComputerManagement;

public static class CreateUpdateComputer
{
    public static async Task<IResult> HandleCreateUpdate([FromBody] ComputerDto computer, IJsonFileRepository repository)
    {
        // ⚠️ Note: The DTO from the HTTP request body MUST BE THE FIRST argument of the endpoint.

        // ⚠️ Note that this method returns different results for different scenarios. Therefore,
        // IResult is the correct return type.

        var macWithoutSeparators = computer.MacAddress.Replace(":", "");

        await using var stream = await repository.Open(macWithoutSeparators, forWriting: true);
        if (stream == null)
        {
            // Computer doesn't exist - create new

            // ⚠️ Note that in practice, AutoMapper (https://automapper.org/) is often used to map DTOs to 
            // other data structures. You are NOT required to use AutoMapper. However, if you are familiar with it,
            // you can use it.
            var computerEntity = new Computer
            {
                MacAddress = computer.MacAddress,
                IpAddress = computer.IpAddress,
                Hostname = computer.Hostname,
                Cpu = computer.Cpu,
                RamGb = computer.RamGb,
                DiskSizeGb = computer.DiskSizeGb,
                Os = computer.Os,
                LastUpdated = DateTime.UtcNow
            };

            // ⚠️ Note While the DTO and data model are currently similar, we should maintain
            // separation between API and data access layers. In a production environment, you should consider:
            //
            // 1. Create separate response DTOs
            // 2. Use mapping (manual or via AutoMapper) between layers
            // 3. Never expose internal data models directly through the API
            //
            // Use this approach (returning the data model of the data access layer) only for simple applications
            // where it is very likely that the DTO and data model are identical. Once requirements change,
            // introduce a separate response DTO for the API layer.
            var item = await repository.Create(macWithoutSeparators, computerEntity);
            return Results.Created($"/computers/{item.Id}", computerEntity);
        }
        else
        {
            // Computer exists - update
            var existingComputer = await repository.Get<Computer>(stream);

            existingComputer.IpAddress = computer.IpAddress;
            existingComputer.Hostname = computer.Hostname;
            existingComputer.Cpu = computer.Cpu;
            existingComputer.RamGb = computer.RamGb;
            existingComputer.DiskSizeGb = computer.DiskSizeGb;
            existingComputer.Os = computer.Os;
            existingComputer.DecommissionedAt = null; // Reset decommissioned status
            existingComputer.LastUpdated = DateTime.UtcNow;

            await repository.Update(stream, existingComputer);
            return Results.Ok(existingComputer);
        }
    }

    #region DTOs
    public class ComputerDto
    {
        // Note that `required` ensures that the user of the API MUST provide a value for the property.

        public required string MacAddress { get; set; }
        public required string IpAddress { get; set; }
        public required string Hostname { get; set; }
        public required string Cpu { get; set; }
        public required decimal RamGb { get; set; }
        public required decimal DiskSizeGb { get; set; }
        public required DataAccess.OperatingSystem Os { get; set; }
    }
    #endregion

    #region Validation
    public static Dictionary<string, string[]> ValidateComputerDto(ComputerDto computer)
    {
        // Create a dictionary to store validation errors. The key is the property name 
        // and the value is an array of error messages. If the validation passes, an empty 
        // dictionary is returned.
        var errors = new Dictionary<string, string[]>();

        // MAC Address validation
        if (string.IsNullOrWhiteSpace(computer.MacAddress))
        {
            errors[nameof(computer.MacAddress)] = ["MAC address is required"];
        }
        else if (!ValidationHelpers.MacAddressRegex().IsMatch(computer.MacAddress))
        {
            errors[nameof(computer.MacAddress)] = ["MAC address must be in format XX:XX:XX:XX:XX:XX where X is a hex digit"];
        }

        // IP Address validation
        if (string.IsNullOrWhiteSpace(computer.IpAddress))
        {
            errors[nameof(computer.IpAddress)] = ["IP address is required"];
        }
        else if (!ValidationHelpers.IsValidIpAddress(computer.IpAddress))
        {
            errors[nameof(computer.IpAddress)] = ["IP address must be in format XXX.XXX.XXX.XXX where X is a digit and XXX is between 0 and 255"];
        }

        // Hostname validation
        if (string.IsNullOrWhiteSpace(computer.Hostname))
        {
            errors[nameof(computer.Hostname)] = ["Hostname is required"];
        }

        // CPU validation
        if (string.IsNullOrWhiteSpace(computer.Cpu))
        {
            errors[nameof(computer.Cpu)] = ["CPU information is required"];
        }

        // RAM validation
        if (computer.RamGb <= 0)
        {
            errors[nameof(computer.RamGb)] = ["RAM size must be greater than 0 GB"];
        }
        else if (computer.RamGb > 10000)
        {
            errors[nameof(computer.RamGb)] = ["RAM size must be less than or equal to 10000 GB"];
        }
        else if (computer.RamGb != Math.Round(computer.RamGb, 2))
        {
            errors[nameof(computer.RamGb)] = ["RAM size must have at most 2 decimal places"];
        }

        // Disk Size validation
        if (computer.DiskSizeGb <= 0)
        {
            errors[nameof(computer.DiskSizeGb)] = ["Disk size must be greater than 0 GB"];
        }
        else if (computer.DiskSizeGb > 100000)
        {
            errors[nameof(computer.DiskSizeGb)] = ["Disk size must be less than or equal to 100000 GB"];
        }
        else if (computer.DiskSizeGb != Math.Round(computer.DiskSizeGb, 2))
        {
            errors[nameof(computer.DiskSizeGb)] = ["Disk size must have at most 2 decimal places"];
        }

        // OS validation
        if (!Enum.IsDefined(computer.Os))
        {
            errors[nameof(computer.Os)] = ["Invalid operating system value"];
        }

        return errors;
    }
 
    #endregion
}