using System.Text.RegularExpressions;

namespace SoftwareInventoryApi.Apis;

/// <summary>
/// Provides pre-compiled regular expressions for validating common formats in software inventory management.
/// This class uses C# source generators ([GeneratedRegex]) to compile the regex patterns at build time,
/// which improves runtime performance compared to creating new Regex instances at runtime.
/// </summary>
/// <remarks>
/// ⚠️ Students: You should be able to write simple Regex patterns from scratch during exams.
/// However, you do NOT need to know the syntax of GeneratedRegex by heart. You will get this file as
/// as an example. If you struggle with Regex, you can also do manual validation (slight reduction of points).
/// </remarks>
public static partial class ValidationHelpers
{
    /// <summary>
    /// Validates IPv4 addresses in the standard dotted decimal notation, like:
    /// - 192.168.1.1
    /// - 10.0.0.1
    ///
    /// The pattern ensures:
    /// - Four groups of numbers separated by dots
    /// - Each group can be 1-3 digits long
    /// Note: This is a basic validation and does not verify that each number is within the valid range (0-255)
    /// </summary>
    // [GeneratedRegex(@"^(\d{1,3}\.){3}\d{1,3}$")]
    // public static partial Regex IpAddressRegex();

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
