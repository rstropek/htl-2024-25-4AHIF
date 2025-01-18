using Gardens.DataAccess;
using Gardens.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Gardens.Apis.GardenAssignmentManagement;

public static class GardenUnassigner
{
    public static IResult HandleUnassignment([FromRoute] string gardenName, GardenNameConverter gardenNameConverter, IJsonFileRepository repository)
    {
        var gardenCoordinates = gardenNameConverter.ConvertGardenNameToCoordinates(gardenName);
        if (gardenCoordinates == null)
        {
            return ValidationHelpers.GetInvalidGardenNameProblemDetails(gardenName);
        }

        if (!repository.EnumerateAll().Any(file => file.Id == gardenCoordinates.ToString()))
        {
            return ValidationHelpers.GetGardenNotAssignedProblemDetails(gardenName);
        }

        repository.Delete(gardenCoordinates.ToString()!);
        return Results.NoContent();
    }
}
