using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Registration.Api.DataAccess;

namespace Registration.Api.Apis.Admin;

public static partial class AdminApi
{
    private static async Task<IResult> GetCampaign([FromRoute] Guid campaignId, IJsonFileRepository repository)
    {
        using var campaignStream = repository.Open(campaignId);
        if (campaignStream is null)
        {
            return Results.NotFound();
        }

        var campaign = await repository.Get<Campaign>(campaignStream)
            ?? throw new InvalidOperationException($"Could not read campaign with id {campaignId} from repository");

        return Results.Ok(campaign);
    }

    private static Ok<IEnumerable<Guid>> GetCampaigns(IJsonFileRepository repository)
        => TypedResults.Ok(repository.EnumerateAll().Select(item => item.Id));
}
