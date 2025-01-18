using Microsoft.AspNetCore.Mvc;

namespace Gardens.Apis.GardenAssignmentManagement;

public static partial class GardenAssignmentManagementApi
{
    public static IEndpointRouteBuilder MapGardenAssignmentManagementApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/gardens/{gardenName}", GardenAssigner.HandleAssignment)
            .AddEndpointFilter(ValidationHelpers.GetEndpointFilter<GardenAssigner.MemberDto>(GardenAssigner.ValidateMember))
            .WithName(nameof(GardenAssigner.HandleAssignment))
            .WithDescription("Assigns a garden to a member")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<DataAccess.Member>(StatusCodes.Status201Created);

        app.MapDelete("/gardens/{gardenName}", GardenUnassigner.HandleUnassignment)
            .WithName(nameof(GardenUnassigner.HandleUnassignment))
            .WithDescription("Unassigns a garden from a member")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent);

        app.MapGet("/gardens/{gardenName}", AssignmentGetter.HandleGetAssignment)
            .WithName(nameof(AssignmentGetter.HandleGetAssignment))
            .WithDescription("Gets the member assigned to a garden")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<DataAccess.Member>(StatusCodes.Status200OK);

        app.MapGet("/gardens/{gardenName}/members", MemberNotifyer.HandleMemberNotification)
            .WithName(nameof(MemberNotifyer.HandleMemberNotification))
            .WithDescription("Gets the members assigned to a garden")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<IEnumerable<string>>(StatusCodes.Status200OK);

        return app;
    }
}