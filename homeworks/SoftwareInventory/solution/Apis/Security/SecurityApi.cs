using Microsoft.AspNetCore.Mvc;
using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.Security;

public static partial class SecurityApi
{
    public static IEndpointRouteBuilder MapSecurityApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/computers/find-installations", FindOutdatedApi.HandleFindInstallation)
            .AddEndpointFilter(ValidationHelpers.GetEndpointFilter<FindOutdatedApi.FindInstallationRequest>(FindOutdatedApi.ValidateFindInstallationRequest))
            .WithName(nameof(FindOutdatedApi.HandleFindInstallation))
            .WithDescription("Finds computers where a certain software is installed")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<Computer[]>(StatusCodes.Status200OK);

        return app;
    }
}

