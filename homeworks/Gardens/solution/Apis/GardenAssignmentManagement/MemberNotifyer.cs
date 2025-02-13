using Gardens.DataAccess;
using Gardens.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Gardens.Apis.GardenAssignmentManagement;

public static class MemberNotifyer
{
    public static async Task<IResult> HandleMemberNotification([FromRoute] string gardenName, GardenNameConverter gardenNameConverter, IJsonFileRepository repository)
    {
        var gardenCoordinates = gardenNameConverter.ConvertGardenNameToCoordinates(gardenName);
        if (gardenCoordinates == null)
        {
            return ValidationHelpers.GetInvalidGardenNameProblemDetails(gardenName);
        }

        var emailAddresses = new List<string>();
        var gardens = repository.EnumerateAll().Select(garden => garden.Id).ToHashSet();
        (int dx, int dy)[] directions = [ (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) ];
        foreach (var direction in directions)
        {
            var neighborCoordinates = gardenCoordinates.Value + direction;
            if (neighborCoordinates.X is < 1 or > 5 || neighborCoordinates.Y is < 1 or > 8 || !gardens.Contains(neighborCoordinates.ToString()))
            {
                continue;
            }

            await using var stream = await repository.Open(neighborCoordinates.ToString()!, false);
            if (stream != null)
            {
                var member = await repository.Get<Member>(stream);
                emailAddresses.Add(member.Email);
            }
        }

        return Results.Ok(emailAddresses);
    }
}
