using Gardens.DataAccess;
using Gardens.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Gardens.Apis.GardenAssignmentManagement;

public static class AssignmentGetter
{
    public static async Task<IResult> HandleGetAssignment([FromRoute] string gardenName, GardenNameConverter gardenNameConverter, IJsonFileRepository repository)
    {
        var gardenCoordinates = gardenNameConverter.ConvertGardenNameToCoordinates(gardenName);
        if (gardenCoordinates == null)
        {
            return ValidationHelpers.GetInvalidGardenNameProblemDetails(gardenName);
        }

        await using var stream = await repository.Open(gardenCoordinates.ToString()!, false);
        if (stream == null)
        {
            return ValidationHelpers.GetGardenNotAssignedProblemDetails(gardenName);
        }

        var member = await repository.Get<Member>(stream);
        return Results.Ok(member);
    }
}
