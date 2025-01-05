using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Registration.Api.DataAccess;

namespace Registration.Api.Apis.Admin;

public static partial class AdminApi
{
    private static async Task<IResult> ActivateCampaign([FromRoute] Guid campaignId, IJsonFileRepository repository, IOptions<ErrorHandlingOptions> settings)
    {
        using var campaignStream = repository.Open(campaignId);
        if (campaignStream is null)
        {
            // Not found is self-explanatory, so we do not need to add a problem details object.
            return Results.NotFound();
        }

        var campaign = await repository.Get<Campaign>(campaignStream)
            ?? throw new InvalidOperationException($"Could not read campaign with id {campaignId} from repository");

        if (campaign.Status == CampaignStatus.Active)
        {
            // If campaign is already active, do nothing
            return Results.Ok();
        }

        if (!campaign.Dates.Any(date => date.Status == CampaignDateStatus.Active))
        {
            // Bad request alone would not be self-explanatory, so we add a problem details object.
            return Results.Problem(new ProblemDetails
            {
                Type = $"{settings.Value.ProblemDetailsUriPrefix}/conflict",
                Title = "Campaign has no active dates",
                Status = StatusCodes.Status409Conflict,
                Instance = campaignId.ToString()
            });
        }

        campaign.Status = CampaignStatus.Active;
        await repository.Update(campaignStream, campaign);

        return Results.Ok();
    }
}

public record ActivateCampaignRequest(Guid CampaignId);
