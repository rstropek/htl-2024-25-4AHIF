using Gardens.DataAccess;
using Gardens.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Gardens.Apis.Accounting;

public static class FertilityPointGetter
{
    public static IResult HandleGetFertilityPoints([FromRoute] string gardenName, GardenNameConverter gardenNameConverter, IJsonFileRepository repository)
    {
        var gardenCoordinates = gardenNameConverter.ConvertGardenNameToCoordinates(gardenName);
        if (gardenCoordinates == null)
        {
            return ValidationHelpers.GetInvalidGardenNameProblemDetails(gardenName);
        }

        return Results.Ok(Fertility.GetFertilityPoints(gardenCoordinates.Value));
    }
}