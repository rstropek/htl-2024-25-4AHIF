using System.Text.RegularExpressions;
using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.ComputerManagement;

public static class CreateUpdateComputer
{
    public static async Task<IResult> HandleCreateUpdate(ComputerDto computer, IJsonFileRepository repository)
    {
        var id = computer.MacAddress.Replace(":", "");

        await using var stream = await repository.Open(id, forWriting: true);
        if (stream == null)
        {
            // Computer doesn't exist - create new
            var computerEntity = new DataAccess.Computer
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

            // ⚠️ Note ⚠️ that in practice there is a HUGE discussion whether it is a good idea to
            // return the data structure (Computer) from the data access layer to the caller.
            // The argument is that the caller should not know about the internal data structure
            // of the data access layer. However, in this rather simple example, the DTO in the
            // API layer would be identical to the data structure in the data access layer.
            // Therefore, it is OK to return the data structure from the data access layer to the caller.
            // However, if the model of the data access layer would contain data that should not be
            // exposed to the caller, then we would need to create a result DTO for the API layer
            // and copy the data from the data access layer to the result DTO.

            var item = await repository.Create(id, computerEntity);
            return Results.Created($"/computers/{item.Id}", computerEntity);
        }
        else
        {
            // Computer exists - update
            var existingComputer = await repository.Get<DataAccess.Computer>(stream) ?? throw new InvalidOperationException("Failed to read existing computer data");

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
    public static async ValueTask<object?> ValidateComputerDto(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var computer = context.GetArgument<ComputerDto>(0);
        var errors = new Dictionary<string, string[]>();

        // MAC Address validation
        if (string.IsNullOrWhiteSpace(computer.MacAddress))
        {
            errors["MacAddress"] = ["MAC address is required"];
        }
        else if (!Regex.IsMatch(computer.MacAddress, @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$"))
        {
            errors["MacAddress"] = ["MAC address must be in format XX:XX:XX:XX:XX:XX where X is a hex digit"];
        }

        // IP Address validation
        if (string.IsNullOrWhiteSpace(computer.IpAddress))
        {
            errors["IpAddress"] = ["IP address is required"];
        }

        // Hostname validation
        if (string.IsNullOrWhiteSpace(computer.Hostname))
        {
            errors["Hostname"] = ["Hostname is required"];
        }

        // CPU validation
        if (string.IsNullOrWhiteSpace(computer.Cpu))
        {
            errors["Cpu"] = new[] { "CPU information is required" };
        }

        // RAM validation
        if (computer.RamGb <= 0)
        {
            errors["RamGb"] = ["RAM size must be greater than 0 GB"];
        }
        else if (computer.RamGb > 10000)
        {
            errors["RamGb"] = ["RAM size must be less than or equal to 10000 GB"];
        }
        else if (computer.RamGb != Math.Round(computer.RamGb, 2))
        {
            errors["RamGb"] = ["RAM size must have at most 2 decimal places"];
        }

        // Disk Size validation
        if (computer.DiskSizeGb <= 0)
        {
            errors["DiskSizeGb"] = ["Disk size must be greater than 0 GB"];
        }
        else if (computer.DiskSizeGb > 100000)
        {
            errors["DiskSizeGb"] = ["Disk size must be less than or equal to 100000 GB"];
        }
        else if (computer.DiskSizeGb != Math.Round(computer.DiskSizeGb, 2))
        {
            errors["DiskSizeGb"] = ["Disk size must have at most 2 decimal places"];
        }

        // OS validation
        if (!Enum.IsDefined(computer.Os))
        {
            errors["Os"] = ["Invalid operating system value"];
        }

        if (errors.Any())
        {
            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
    #endregion
}