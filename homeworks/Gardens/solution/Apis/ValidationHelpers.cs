using Microsoft.AspNetCore.Mvc;

namespace Gardens.Apis;

public static partial class ValidationHelpers
{
    #region Helpers for generating problem details for frequently occurring errors
    public static IResult GetInvalidGardenNameProblemDetails(string gardenName)
    {
        return Results.BadRequest(new ProblemDetails
        {
            Title = "Invalid garden name",
            Detail = $"The garden name '{gardenName}' is not valid."
        });
    }

    public static IResult GetGardenNotAssignedProblemDetails(string gardenName)
    {
        return Results.NotFound(new ProblemDetails
        {
            Title = "Garden not assigned",
            Detail = $"The garden '{gardenName}' is not assigned."
        });
    }

    public static IResult GetGardenAlreadyAssignedProblemDetails(string gardenName)
    {
        return Results.Conflict(new ProblemDetails
        {
            Title = "Garden already assigned",
            Detail = $"The garden '{gardenName}' is already assigned."
        });
    }
    #endregion

    /// <summary>
    /// Creates an endpoint filter that validates a request DTO using the provided validation function.
    /// </summary>
    /// <typeparam name="T">The type of the request DTO to validate</typeparam>
    /// <param name="validationResult">A function that takes the DTO and returns a dictionary of validation errors.
    /// The dictionary keys are property names and values are arrays of error messages.</param>
    /// <returns>An endpoint filter delegate</returns>
    /// <remarks>
    /// ⚠️ This filter assumes the DTO is the FIRST argument of the endpoint method.
    /// The validation function should return an empty dictionary if validation passes.
    /// ⚠️ Students: You do NOT need to write this helper method by hand. It will be part of the starter
    /// code during exams.
    /// </remarks>
    public static Func<EndpointFilterInvocationContext, EndpointFilterDelegate, ValueTask<object?>> GetEndpointFilter<T>(
        Func<T, Dictionary<string, string[]>> validationResult)
    {
        return async (context, next) =>
        {
            var computer = context.GetArgument<T>(0);
            var errors = validationResult(computer);
            if (errors.Count > 0)
            {
                return Results.ValidationProblem(errors);
            }

            return await next(context);
        };
    }

}
