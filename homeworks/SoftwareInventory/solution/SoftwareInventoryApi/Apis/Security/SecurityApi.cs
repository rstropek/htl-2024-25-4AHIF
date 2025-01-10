using Microsoft.AspNetCore.Mvc;
using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.Security;

public static partial class SecurityApi
{
    public static IEndpointRouteBuilder MapSecurityApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("computers");

        api.MapGet("/find-outdated", FindOutdatedApi.HandleFindOutdated)
            .WithName(nameof(FindOutdatedApi.HandleFindOutdated))
            .WithDescription("Finds computers with outdated software")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<Computer[]>(StatusCodes.Status200OK);

        return app;
    }
}

