using Microsoft.AspNetCore.Mvc;
using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.Security;

public static partial class SecurityApi
{
    public static IEndpointRouteBuilder MapSecurityApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/computers/find-outdated", FindOutdatedApi.HandleFindOutdated)
            .AddEndpointFilter(ValidationHelpers.GetEndpointFilter<FindOutdatedApi.FindOutdatedRequest>(FindOutdatedApi.ValidateFindOutdatedRequest))
            .WithName(nameof(FindOutdatedApi.HandleFindOutdated))
            .WithDescription("Finds computers with outdated software")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<Computer[]>(StatusCodes.Status200OK);

        return app;
    }
}

