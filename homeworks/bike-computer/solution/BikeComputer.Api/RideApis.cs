using BikeComputer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeComputer.Logic;

namespace BikeComputer.Api;

public static partial class RideApis
{
    public static IEndpointRouteBuilder MapRideApis(this IEndpointRouteBuilder app)
    {
        app.MapPost("/rides", HandleUploadRide)
            .WithName(nameof(HandleUploadRide))
            .WithDescription("Uploads a ride")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<NewRideResultDto>(StatusCodes.Status201Created);

        app.MapGet("/rides", HandleGetRides)
            .WithName(nameof(HandleGetRides))
            .WithDescription("Gets all rides")
            .Produces<List<RideDto>>(StatusCodes.Status200OK);

        app.MapGet("/rides/{id}", HandleGetRide)
            .WithName(nameof(HandleGetRide))
            .WithDescription("Gets a ride by ID")
            .Produces<RideDetailsDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        app.MapDelete("/rides/{id}", HandleDeleteRide)
            .WithName(nameof(HandleDeleteRide))
            .WithDescription("Deletes a ride")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent);

        app.MapPatch("/rides/{id}", HandleUpdateRide)
            .AddEndpointFilter(ValidationHelpers.GetEndpointFilter<UpdateRideDto>(ValidateUpdateRideDto))
            .WithName(nameof(HandleUpdateRide))
            .WithDescription("Updates a ride")
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK);

        return app;
    }

    #region Upload Ride
    private static async Task<IResult> HandleUploadRide([FromBody] NewRideDto newRide, ApplicationDataContext context)
    {
        var bike = await context.Bikes
            .FirstOrDefaultAsync(b => b.SerialNumberBikeComputer == newRide.SerialNumberBikeComputer);

        if (bike == null)
        {
            return Results.Problem("Bike not found", statusCode: StatusCodes.Status404NotFound);
        }

        RideAnalyzer.AnalysisResult analysisResult;
        try
        {
            analysisResult = RideAnalyzer.AnalyzeRide(newRide.Timestamps, bike.Circumference_mm);
        }
        catch (ArgumentException ex)
        {
            return Results.Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }

        var ride = new Ride
        {
            BikeId = bike.Id,
            Title = $"Ride on {newRide.Timestamps.First():yyyy-MM-dd}",
            UploadedAt = DateTimeOffset.UtcNow,
            RideDuration_s = analysisResult.RideDuration_s,
            RideDistance_m = analysisResult.RideDistance_m,
            AvgSpeed_kmh = analysisResult.AvgSpeed_kmh,
            NumberOfStops = analysisResult.NumberOfStops,
            TotalStopTime_s = analysisResult.TotalStopTime_s
        };

        context.Rides.Add(ride);
        await context.SaveChangesAsync();

        var result = new NewRideResultDto
        {
            Id = ride.Id,
            UploadedAt = ride.UploadedAt,
            Title = ride.Title
        };
        return Results.Created($"/rides/{ride.Id}", result);
    }

    public class NewRideDto
    {
        public required string SerialNumberBikeComputer { get; set; }
        public required DateTimeOffset[] Timestamps { get; set; }
    }

    public class NewRideResultDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required DateTimeOffset UploadedAt { get; set; }
    }
    #endregion

    #region Get Rides
    private static async Task<IResult> HandleGetRides(ApplicationDataContext context)
    {
        var rides = await context.Rides
            .Select(r => new RideDto
            {
                Id = r.Id,
                Title = r.Title,
                UploadedAt = r.UploadedAt,
            })
            .ToListAsync();

        return Results.Ok(rides);
    }

    public class RideDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required DateTimeOffset UploadedAt { get; set; }
    }
    #endregion

    #region Get Ride
    private static async Task<IResult> HandleGetRide(int id, ApplicationDataContext context)
    {
        var ride = await context.Rides
            .Where(r => r.Id == id)
            .Select(r => new RideDetailsDto
            {
                Id = r.Id,
                Title = r.Title,
                UploadedAt = r.UploadedAt,
                RideDuration_s = r.RideDuration_s,
                RideDistance_m = r.RideDistance_m,
                AvgSpeed_kmh = r.AvgSpeed_kmh,
                NumberOfStops = r.NumberOfStops,
                TotalStopTime_s = r.TotalStopTime_s
            })
            .FirstOrDefaultAsync();

        if (ride == null)
        {
            return Results.Problem("Ride not found", statusCode: StatusCodes.Status404NotFound);
        }

        return Results.Ok(ride);
    }
    public class RideDetailsDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required DateTimeOffset UploadedAt { get; set; }
        public required float RideDuration_s { get; set; }
        public required float RideDistance_m { get; set; }
        public required float AvgSpeed_kmh { get; set; }
        public required int NumberOfStops { get; set; }
        public required float TotalStopTime_s { get; set; }
    }
    #endregion

    #region Delete Ride
    private static async Task<IResult> HandleDeleteRide(int id, ApplicationDataContext context)
    {
        var ride = await context.Rides.FindAsync(id);
        if (ride == null)
        {
            return Results.Problem("Ride not found", statusCode: StatusCodes.Status404NotFound);
        }

        context.Rides.Remove(ride);
        await context.SaveChangesAsync();

        return Results.NoContent();
    }
    #endregion

    #region Update Ride
    private static async Task<IResult> HandleUpdateRide([FromBody] UpdateRideDto updateRide, [FromRoute] int id, ApplicationDataContext context)
    {
        var ride = await context.Rides.FindAsync(id);
        if (ride == null)
        {
            return Results.Problem("Ride not found", statusCode: StatusCodes.Status404NotFound);
        }

        ride.Title = updateRide.Title;
        await context.SaveChangesAsync();

        return Results.Ok();
    }

    public class UpdateRideDto
    {
        public required string Title { get; set; }
    }

    public static Dictionary<string, string[]> ValidateUpdateRideDto(UpdateRideDto updateRide)
    {
        // Create a dictionary to store validation errors. The key is the property name 
        // and the value is an array of error messages. If the validation passes, an empty 
        // dictionary is returned.
        var errors = new Dictionary<string, string[]>();

        // Validate the Title property
        if (string.IsNullOrWhiteSpace(updateRide.Title))
        {
            errors[nameof(updateRide.Title)] = ["Title is required."];
        }

        return errors;
    }
    #endregion
}
