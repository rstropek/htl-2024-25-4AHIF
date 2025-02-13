using Gardens.DataAccess;
using Gardens.Logic;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Gardens.Apis.Accounting;

public static class FeeCalculator
{
    public static async Task<Ok<IEnumerable<FeesResponse>>> HandleGetFees(IJsonFileRepository repository)
    {
        var result = new Dictionary<string, FeesResponse>();
        for (var y = 1; y <= 8; y++)
        {
            for (var x = 1; x <= 5; x++)
            {
                var coordinates = new GardenCoordinates(x, y);
                using var stream = await repository.Open(coordinates.ToString(), false);
                if (stream != null)
                {
                    var member = await repository.Get<Member>(stream);
                    var gardenFee = 8m * Fertility.GetFertilityPoints(coordinates) * 3m;
                    if (result.TryGetValue(member.Name, out var memberFee))
                    {
                        // Note how we use `with` to update the record. If you are not familiar with this, do a little research.
                        result[member.Name] = memberFee with { Fee = memberFee.Fee + gardenFee };
                    }
                    else
                    {
                        result[member.Name] = new FeesResponse(member.Name, member.Email, gardenFee);
                    }
                }
            }
        }

        return TypedResults.Ok(result.Values.AsEnumerable());
    }

    public record FeesResponse(string Name, string Email, decimal Fee);
}