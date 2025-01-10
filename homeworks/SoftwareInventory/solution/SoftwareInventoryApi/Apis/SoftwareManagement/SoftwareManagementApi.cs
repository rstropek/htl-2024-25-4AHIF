using Microsoft.AspNetCore.Mvc;

namespace SoftwareInventoryApi.Apis.SoftwareManagement;

public static partial class SoftwareManagementApi
{
    public static IEndpointRouteBuilder MapSoftwareManagementApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("computers");

        api.MapPost("/{computerMacAddress}/software", AddSoftwareApi.HandleAddSoftware)
            .AddEndpointFilter(AddSoftwareApi.ValidateAddSoftwareDto)
            .WithName(nameof(AddSoftwareApi.HandleAddSoftware))
            .WithDescription("Add software to a computer")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<DataAccess.Computer>(StatusCodes.Status200OK);

        return app;
    }
}
