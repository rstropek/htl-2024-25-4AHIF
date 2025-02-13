using Microsoft.AspNetCore.Mvc;

namespace Gardens.Apis.Accounting;

public static partial class AccountingApi
{
    public static IEndpointRouteBuilder MapAccountingApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/gardens/{gardenName}/fertility-points", FertilityPointGetter.HandleGetFertilityPoints)
            .WithName(nameof(FertilityPointGetter.HandleGetFertilityPoints))
            .WithDescription("Gets the fertility points of a garden")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<decimal>(StatusCodes.Status200OK);

        app.MapGet("/gardens/fees", FeeCalculator.HandleGetFees)
            .WithName(nameof(FeeCalculator.HandleGetFees))
            .WithDescription("Gets the fees for all members")
            .Produces<IEnumerable<FeeCalculator.FeesResponse>>(StatusCodes.Status200OK);

        return app;
    }
}