using Microsoft.AspNetCore.Mvc;
using Registration.Api.CrossCuttingConcerns;
using Registration.Api.DataAccess;

namespace Registration.Api.Apis.Admin;

public static partial class AdminApi
{
    public static IServiceCollection AddAdminApi(this IServiceCollection services)
        => services
            .AddSingleton<IDtoValidator<CreateCampaignRequest>, CreateCampaignValidator>()
            .AddSingleton<IDtoValidator<CreateDateRequest>, CreateDateValidator>()
            .AddSingleton<IDtoValidator<DepartmentAssignmentRequest>, DepartmentAssignmentValidator>();

    public static IEndpointRouteBuilder MapAdminApi(this IEndpointRouteBuilder app)
    {
        // Note that we attach the MapAdminApi method to IEndpointRouteBuilder.
        // This interface is the core abstraction for building routes in ASP.NET Core.

        // Create a new route group with 'admin' prefix. All routes added to this group will start with 'admin'.
        var api = app.MapGroup("admin");

        // Note that it is good practice to add a name, summary, etc. to each endpoint.
        // Note the presence of the validation filter. It ensures that the DTO is validated before the endpoint is called.
        api.MapPost("/campaigns", CreateCampaign)
            .AddValidationFilter<CreateCampaignRequest>()
            .WithName(nameof(CreateCampaign))
            .WithSummary("Create a new campaign including initial dates and department assignments")
            .WithDescription("""
                Creates a new campaign including initial dates and department assignments.
                The campaign is inactive by default and must be activated explicitly.
                """)
            .Produces<CreateCampaignResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    

        api.MapPost("/campaigns/{campaignId}/activate", ActivateCampaign)
            .WithName("ActivateCampaign")
            .WithSummary("Activate a campaign")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        api.MapDelete("/campaigns/{campaignId}", DeleteCampaign)
            .WithName("DeleteCampaign")
            .WithSummary("Delete a campaign")
            .WithDescription("Deletes a campaign if it has no existing registrations")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        api.MapGet("/campaigns", GetCampaigns)
            .WithName("GetCampaigns")
            .WithSummary("Get all campaigns")
            .Produces<IEnumerable<Guid>>(StatusCodes.Status200OK);

        api.MapGet("/campaigns/{campaignId}", GetCampaign)
            .WithName("GetCampaign")
            .WithSummary("Get a campaign")
            .Produces<Campaign>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        // Return the app so that we can chain other methods to it.
        return app;
    }
}
