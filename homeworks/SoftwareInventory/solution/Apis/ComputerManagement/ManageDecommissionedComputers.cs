using Microsoft.AspNetCore.Http.HttpResults;
using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.ComputerManagement;

public static class ManageDecommissionedComputers
{
    public static async Task<Ok<List<string>>> HandleDecommissionCheck(IJsonFileRepository repository)
    {
        // ⚠️ Note that this method always returns Ok with a list of decommissioned computers (mac addresses).
        // Therefore, Ok is the correct return type and TypedResults.Ok (see below) is the correct method to use.

        var decommissionThreshold = DateTimeOffset.UtcNow.AddDays(-30);
        var decommissionedMacs = new List<string>();
        
        var computers = repository.EnumerateAll();
        foreach (var computerId in computers)
        {
            var stream = await repository.Open(computerId.Id, forWriting: true);
            if (stream == null) { continue; } // Ignore computer if the file cannot be opened

            var computer = await repository.Get<Computer>(stream);
            if (computer.LastUpdated <= decommissionThreshold)
            {
                computer.DecommissionedAt = DateTimeOffset.UtcNow;
                await repository.Update(stream, computer);
                decommissionedMacs.Add(computer.MacAddress);
            }
        }

        return TypedResults.Ok(decommissionedMacs);
    }
} 