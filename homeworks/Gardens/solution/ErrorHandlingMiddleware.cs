using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Gardens;

/// <summary>
/// This middleware handles all unhandled exceptions that occur during the processing of HTTP requests in the application.
/// 
/// In a web application, many things can go wrong - from invalid JSON in request bodies to database failures
/// or other unexpected errors. Without proper error handling, these issues would result in unclear or unsafe
/// error messages being sent to clients.
/// 
/// This middleware acts like a safety net that catches any exceptions that weren't handled elsewhere in the application.
/// It sits in the HTTP request pipeline and:
/// 1. Allows the request to proceed normally if no errors occur
/// 2. Catches any exceptions that happen during processing
/// 3. Converts these exceptions into standardized, client-friendly error responses
/// 
/// Specifically, it handles:
/// - Bad JSON in request bodies (returns a 400 Bad Request with details about the JSON error)
/// - All other unexpected errors (returns a 500 Internal Server Error, logs the full error details securely,
///   and returns a safe error message to the client)
/// 
/// This centralized error handling ensures that:
/// - Clients always receive well-formatted error responses
/// - Sensitive error details are never leaked to clients
/// - All unexpected errors are properly logged for debugging
/// - The application maintains consistent error handling across all endpoints
/// 
/// ⚠️ For students: You do NOT need to be able to write this code from scratch during exams. It will be
/// part of your starter code. However, maybe the code is useful for your projects. You can copy and adjust
/// it to your needs.
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadHttpRequestException ex) when (ex.InnerException is JsonException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Bad Request",
                Detail = ex.InnerException.Message
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "Internal Server Error",
                Detail = $"An unexpected error occurred. Check system log for request id {context.TraceIdentifier}"
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}