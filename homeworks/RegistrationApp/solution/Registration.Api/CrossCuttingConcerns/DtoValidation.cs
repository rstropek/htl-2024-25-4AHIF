namespace Registration.Api.CrossCuttingConcerns;

/// <summary>
/// Defines a validator interface for validating Data Transfer Objects (DTOs).
/// </summary>
/// <typeparam name="T">The type of DTO to validate.</typeparam>
public interface IDtoValidator<T>
{
    /// <summary>
    /// Validates the provided DTO and returns any validation errors.
    /// </summary>
    /// <param name="dto">The DTO instance to validate.</param>
    /// <returns>
    /// An enumerable collection of <see cref="ValidationError"/> objects representing validation failures.
    /// Returns an empty collection if validation succeeds.
    /// </returns>
    IEnumerable<ValidationError> Validate(T dto);
}

/// <summary>
/// Represents a validation error that occurred during DTO validation.
/// </summary>
/// <param name="Instance">The specific instance or path where the validation error occurred.</param>
/// <param name="Source">The source property or field that caused the validation error.</param>
/// <param name="Message">A descriptive message explaining the validation error.</param>
public record ValidationError(
    string Instance,
    string Source,
    string Message
)
{
    /// <summary>
    /// Creates a new validation error by combining an instance prefix with an existing validation error.
    /// </summary>
    /// <param name="instancePrefix">The prefix to prepend to the source error's instance path.</param>
    /// <param name="source">The source validation error to derive the new error from.</param>
    public ValidationError(string instancePrefix, ValidationError source)
    : this(
        $"{instancePrefix}/{source.Instance}",
        source.Source,
        source.Message
    )
    {
    }
}

/// <summary>
/// Provides an ASP.NET Core Minimal API endpoint filter for validating DTOs.
/// </summary>
/// <remarks>
/// ASP.NET Core Minimal API provides a standardized way to validate input parameters before processing
/// them in endpoint handlers. This helper class provides such a filter for our <see cref="IDtoValidator{T}"/> 
/// implementation. For details about endpoint filters in ASP.NET Core Minimal API 
/// see <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/min-api-filters"/>.
/// </remarks>
public class ValidationResultFilter<T1, T2>(T1 validator, IConfiguration configuration) : IEndpointFilter where T1 : IDtoValidator<T2>
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // ⚠️ IMPORTANT ⚠️
        // The first argument of endpoint handlers must be the DTO to validate. It would be possible to
        // add additional code to verify that the first argument is the DTO to validate (using reflection).
        // However, this would slow our code down and the code would be quite complex. So we stick to the
        // convention that the first argument is the DTO to validate.
        var dto = context.GetArgument<T2>(0);
        var validationResults = validator.Validate(dto).ToArray();
        if (validationResults.Any())
        {
            var problemDetailsUriPrefix = configuration.GetValue<string>("ProblemDetailsUriPrefix") ?? "https://example.com/errors";
            return Results.Problem(
                type: $"{problemDetailsUriPrefix}/validation-failed",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Payload validation failed",
                extensions: new Dictionary<string, object?>
                {
                    ["validationResults"] = validationResults
                }
            );
        }

        return await next(context);
    }
}

/// <summary>
/// Helper function to make it easier to add the validation filter to an endpoint.
/// </summary>
public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder AddValidationFilter<T>(this RouteHandlerBuilder builder)
        => builder.AddEndpointFilter<ValidationResultFilter<IDtoValidator<T>, T>>();
}