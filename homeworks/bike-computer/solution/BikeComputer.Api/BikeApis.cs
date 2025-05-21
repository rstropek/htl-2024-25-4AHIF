using BikeComputer.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeComputer.Api;

public static partial class BikeApis
{
    public static IEndpointRouteBuilder MapBikeApis(this IEndpointRouteBuilder app)
    {
        app.MapPost("/bikes", HandleRegisterBike)
            .AddEndpointFilter(ValidationHelpers.GetEndpointFilter<NewBikeDto>(ValidateNewBikeDto))
            .WithName(nameof(HandleRegisterBike))
            .WithDescription("Registers a new bike")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<NewBikeResultDto>(StatusCodes.Status201Created);

        app.MapGet("/bikes", HandleGetBikes)
            .WithName(nameof(HandleGetBikes))
            .WithDescription("Gets all bikes")
            .Produces<List<GetBikesResultDto>>(StatusCodes.Status200OK);

        return app;
    }

    #region Register Bike
    private static async Task<IResult> HandleRegisterBike([FromBody] NewBikeDto newBike, ApplicationDataContext context)
    {
        int circumference_mm = 0;
        if (newBike.Diameter_mm != null)
        {
            circumference_mm = (int)Math.Round(newBike.Diameter_mm.Value * Math.PI);
        }
        else if (!string.IsNullOrWhiteSpace(newBike.EtrtoDesignation))
        {
            var tireType = TireTypes.FirstOrDefault(t => t.Etreto == newBike.EtrtoDesignation);
            if (tireType != null)
            {
                circumference_mm = tireType.Circumference_mm;
            }
        }

        var bike = new Bike
        {
            Title = newBike.Title,
            SerialNumberBikeComputer = newBike.SerialNumberBikeComputer,
            Circumference_mm = circumference_mm,
        };

        context.Bikes.Add(bike);
        await context.SaveChangesAsync();

        var result = new NewBikeResultDto
        {
            Id = bike.Id,
            Title = bike.Title,
            SerialNumberBikeComputer = bike.SerialNumberBikeComputer,
            Circumference_mm = bike.Circumference_mm
        };
        return Results.Created($"/bikes/{bike.Id}", result);
    }

    public class NewBikeDto
    {
        public required string Title { get; set; }
        public required string SerialNumberBikeComputer { get; set; }
        public string? EtrtoDesignation { get; set; }
        public int? Diameter_mm { get; set; }
    }

    public class NewBikeResultDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string SerialNumberBikeComputer { get; set; }
        public required int Circumference_mm { get; set; }
    }

    private record TireType(
        string Etreto,
        int Circumference_mm
    );

    // To keep things simple, we import the tire types in C#.
    // In a real-world application, this might be read from the DB.
    private static readonly TireType[] TireTypes =
    [
        new("25-559", 1913),
        new("32-559", 1950),
        new("37-559", 2005),
        new("40-559", 2010),
        new("47-559", 2023),
        new("50-559", 2050),
        new("52-559", 2055),
        new("54-559", 2068),
        new("57-559", 2070),
        new("58-559", 2083),
        new("75-559", 2170),
        new("40-584", 2079),
        new("50-584", 2090),
        new("54-584", 2148),
        new("57-584", 2182),
        new("58-584", 2199),
        new("60-584", 2207),
        new("61-584", 2213),
        new("64-584", 2231),
        new("66-584", 2247),
        new("71-584", 2279),
        new("54-622", 2286),
        new("56-622", 2302),
        new("57-622", 2310),
        new("58-622", 2326),
        new("60-622", 2326),
        new("61-622", 2333),
        new("64-622", 2350),
        new("66-622", 2366),
    ];

    public static Dictionary<string, string[]> ValidateNewBikeDto(NewBikeDto newBike)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(newBike.Title))
        {
            errors[nameof(newBike.Title)] = ["Title is required."];
        }

        if (string.IsNullOrWhiteSpace(newBike.SerialNumberBikeComputer))
        {
            errors[nameof(newBike.SerialNumberBikeComputer)] = ["SerialNumberBikeComputer is required."];
        }

        if (newBike.Diameter_mm < 0)
        {
            errors[nameof(newBike.Diameter_mm)] = ["Diameter_mm must be a positive number."];
        }

        if (string.IsNullOrWhiteSpace(newBike.EtrtoDesignation) && newBike.Diameter_mm == null)
        {
            errors[nameof(newBike.EtrtoDesignation)] = ["Either EtrtoDesignation or Diameter_mm must be provided."];
        }

        if (!string.IsNullOrWhiteSpace(newBike.EtrtoDesignation) && newBike.Diameter_mm != null)
        {
            errors[nameof(newBike.EtrtoDesignation)] = ["Either EtrtoDesignation or Diameter_mm must be provided, not both."];
        }

        if (!string.IsNullOrWhiteSpace(newBike.EtrtoDesignation))
        {
            var tireType = TireTypes.FirstOrDefault(t => t.Etreto == newBike.EtrtoDesignation);
            if (tireType == null)
            {
                errors[nameof(newBike.EtrtoDesignation)] = ["Invalid ETRTO Designation."];
            }
        }

        return errors;
    }
    #endregion

    #region Get Bikes
    private static async Task<Ok<List<GetBikesResultDto>>> HandleGetBikes(ApplicationDataContext context)
    {
        var bikes = await context.Bikes
            .Select(b => new 
            {
                b.Id,
                b.Title,
                b.SerialNumberBikeComputer,
                b.ViewedAt,
                RidesUploadedAt = b.Rides.Select(r => r.UploadedAt),
            })
            .ToListAsync();

        var result = bikes.Select(b => new GetBikesResultDto
        {
            Id = b.Id,
            Title = b.Title,
            SerialNumberBikeComputer = b.SerialNumberBikeComputer,
            HasNewRides = b.RidesUploadedAt.Any(r => r > b.ViewedAt)
        }).ToList();

        foreach (var bike in result.Where(b => b.HasNewRides))
        {
            var bikeEntity = await context.Bikes.FindAsync(bike.Id);
            if (bikeEntity != null)
            {
                bikeEntity.ViewedAt = DateTimeOffset.UtcNow;
            }
        }
        await context.SaveChangesAsync();

        return TypedResults.Ok(result);
    }
    
    public class GetBikesResultDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string SerialNumberBikeComputer { get; set; }
        public required bool HasNewRides { get; set; }
    }
    #endregion
}
