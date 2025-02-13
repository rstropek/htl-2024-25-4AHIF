using Microsoft.AspNetCore.Mvc;

namespace SoftwareInventoryApi.Apis.ComputerManagement;

public static partial class ComputerManagementApi
{
    public static IEndpointRouteBuilder MapComputerManagementApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/computers", CreateUpdateComputer.HandleCreateUpdate)
            .AddEndpointFilter(ValidationHelpers.GetEndpointFilter<CreateUpdateComputer.ComputerDto>(CreateUpdateComputer.ValidateComputerDto))
            .WithName(nameof(CreateUpdateComputer.HandleCreateUpdate))
            .WithDescription("Create or update a computer record")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<DataAccess.Computer>(StatusCodes.Status201Created) // New Computer
            .Produces<DataAccess.Computer>(StatusCodes.Status200OK); // Updated Computer

        app.MapPost("/computers/check-decommissioned", ManageDecommissionedComputers.HandleDecommissionCheck)
            .WithName(nameof(ManageDecommissionedComputers.HandleDecommissionCheck))
            .WithDescription("Checks and updates decommissioned status for all computers")
            .Produces(StatusCodes.Status200OK);

        return app;
    }
}
