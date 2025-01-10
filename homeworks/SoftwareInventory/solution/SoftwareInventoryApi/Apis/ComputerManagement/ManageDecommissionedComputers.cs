using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.ComputerManagement;

public static class ManageDecommissionedComputers
{
    public static async Task<IResult> HandleDecommissionCheck(IJsonFileRepository repository)
    {
        var decommissionThreshold = DateTimeOffset.UtcNow.AddDays(-30);
        
        var computers = repository.EnumerateAll();
        foreach (var computerId in computers)
        {
            var stream = await repository.Open(computerId.Id, forWriting: true);
            if (stream == null) { continue; } // Ignore computer if the file cannot be opened

            var computer = await repository.Get<Computer>(stream) ?? throw new InvalidOperationException("Failed to read existing computer data");
            if (computer.LastUpdated <= decommissionThreshold)
            {
                computer.DecommissionedAt = DateTimeOffset.UtcNow;
                await repository.Update(stream, computer);
            }
        }

        return Results.Ok();
    }
} 