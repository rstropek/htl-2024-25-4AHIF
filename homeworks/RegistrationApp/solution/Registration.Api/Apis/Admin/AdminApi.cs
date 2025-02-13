using Microsoft.AspNetCore.Mvc;
using Registration.Api.CrossCuttingConcerns;
using Registration.Api.DataAccess;

namespace Registration.Api.Apis.Admin;

public static partial class AdminApi
{
    public static IServiceCollection AddAdminApi(this IServiceCollection services)
        => services
            .AddSingleton<IDtoValidator<CampaignCreationApi.CreateCampaignRequest>, CampaignCreationApi.CampaignValidator>()
            .AddSingleton<IDtoValidator<CampaignCreationApi.CreateDateRequest>, CampaignCreationApi.DateValidator>()
            .AddSingleton<IDtoValidator<CampaignCreationApi.DepartmentAssignmentRequest>, CampaignCreationApi.DepartmentAssignmentValidator>();

    public static IEndpointRouteBuilder MapAdminApi(this IEndpointRouteBuilder app)
    {
        // Note that we attach the MapAdminApi method to IEndpointRouteBuilder.
        // This interface is the core abstraction for building routes in ASP.NET Core.

        // Create a new route group with 'admin' prefix. All routes added to this group will start with 'admin'.
        var api = app.MapGroup("admin");

        // Note that it is good practice to add a name, summary, etc. to each endpoint.
        // Note the presence of the validation filter. It ensures that the DTO is validated before the endpoint is called.
        api.MapPost("/campaigns", CampaignCreationApi.CreateCampaign)
            .AddValidationFilter<CampaignCreationApi.CreateCampaignRequest>()
            .WithName("CreateCampaign")
            .WithSummary("Create a new campaign including initial dates and department assignments")
            .WithDescription("""
                Creates a new campaign including initial dates and department assignments.
                The campaign is inactive by default and must be activated explicitly.
                """)
            .Produces<CampaignCreationApi.CreateCampaignResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);


        api.MapPost("/campaigns/{campaignId}/activate", CampaignActivationApi.ActivateCampaign)
            .WithName("ActivateCampaign")
            .WithSummary("Activate a campaign")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        api.MapDelete("/campaigns/{campaignId}", CampaignDeletionApi.DeleteCampaign)
            .WithName("DeleteCampaign")
            .WithSummary("Delete a campaign")
            .WithDescription("Deletes a campaign if it has no existing registrations")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        api.MapGet("/campaigns", CampaignRetrievalApi.GetCampaigns)
            .WithName("GetCampaigns")
            .WithSummary("Get all campaigns")
            .Produces<IEnumerable<Guid>>(StatusCodes.Status200OK);

        api.MapGet("/campaigns/{campaignId}", CampaignRetrievalApi.GetCampaign)
            .WithName("GetCampaign")
            .WithSummary("Get a campaign")
            .Produces<Campaign>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        api.MapPatch("/campaigns/{campaignId}", CampaignUpdateApi.UpdateCampaign)
            .WithName("UpdateCampaign")
            .WithSummary("Update a campaign")
            .Produces<Campaign>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

        // Return the app so that we can chain other methods to it.
        return app;
    }
}
