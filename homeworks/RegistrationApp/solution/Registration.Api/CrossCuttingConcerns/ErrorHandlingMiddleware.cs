namespace Registration.Api.CrossCuttingConcerns;

public static class ErrorHandlingMiddleware
{
    /// <summary>
    /// Contains extension methods for handling errors and exceptions in the API pipeline.
    /// </summary>
    /// <remarks>
    /// This middleware provides centralized error handling capabilities with different 
    /// behaviors for development and production environments.
    /// In development, it shows detailed error information to assist debugging.
    /// In production, it returns sanitized error responses without sensitive details.
    /// </remarks>
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app, bool isDevelopment)
    {
        if (isDevelopment)
        {
            // During development, return all exception details to the client
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // During production, return a generic error message to the client.
            // Do NOT return exception details to the client as they may contain sensitive information.
            // Add the traceId to the response so that administrators can use it to find more information in the logs.
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                // Get access to the configuration (appsettings.json) and retrieve the problemDetailsUriPrefix.
                // If the configuration is not found, use a default value.
                var config = app.ApplicationServices.GetService<IConfiguration>();
                var problemDetailsUriPrefix = config?.GetValue<string>("ProblemDetailsUriPrefix") ?? "https://example.com/errors";

                exceptionHandlerApp.Run(async context =>
                {
                    await Results.Problem(
                        type: $"{problemDetailsUriPrefix}/internal-server-error",
                        title: "An error occurred processing your request",
                        statusCode: StatusCodes.Status500InternalServerError,
                        extensions: new Dictionary<string, object?>
                        {
                            ["traceId"] = context.TraceIdentifier
                        }
                    ).ExecuteAsync(context);
                });
            });
        }

        return app;
    }
}