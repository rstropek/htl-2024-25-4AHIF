using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Registration.Api.DataAccess;

namespace Registration.Api.Apis.Admin;

public static partial class AdminApi
{
    private static async Task<IResult> DeleteCampaign([FromRoute] Guid campaignId, IJsonFileRepository repository, IOptions<ErrorHandlingOptions> settings)
    {
        using (var campaignStream = repository.Open(campaignId))
        {
            if (campaignStream is null)
            {
                return Results.NotFound();
            }

            var campaign = await repository.Get<Campaign>(campaignStream)
                ?? throw new InvalidOperationException($"Could not read campaign with id {campaignId} from repository");

            if (campaign.Dates.Any(date => date.DepartmentAssignments.Any(assignment => assignment.Registrations.Any())))
            {
                return Results.Problem(new ProblemDetails
                {
                    Type = $"{settings.Value.ProblemDetailsUriPrefix}/conflict",
                    Title = "Cannot delete campaign with existing registrations",
                    Status = StatusCodes.Status409Conflict,
                    Instance = campaignId.ToString()
                });
            }
        }

        repository.Delete(campaignId);
        return Results.NoContent();
    }
}