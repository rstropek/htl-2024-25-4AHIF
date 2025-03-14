using Microsoft.AspNetCore.Mvc;
using SoftwareInventoryApi.DataAccess;

namespace SoftwareInventoryApi.Apis.SoftwareManagement;

public static partial class SoftwareManagementApi
{
    public static IEndpointRouteBuilder MapSoftwareManagementApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/computers/{computerMacAddress}/software", AddSoftwareApi.HandleAddSoftware)
            .AddEndpointFilter(ValidationHelpers.GetEndpointFilter<AddSoftwareApi.AddSoftwareDto>(AddSoftwareApi.ValidateAddSoftwareDto))
            .WithName(nameof(AddSoftwareApi.HandleAddSoftware))
            .WithDescription("Add software to a computer")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<Computer>(StatusCodes.Status200OK);

        return app;
    }   
}
