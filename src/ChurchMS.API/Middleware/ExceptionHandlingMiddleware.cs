using System.Net;
using System.Text.Json;
using ChurchMS.Application.Exceptions;
using ChurchMS.Shared.Models;

namespace ChurchMS.API.Middleware;

/// <summary>
/// Global exception handling middleware mapping domain exceptions to HTTP status codes.
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            Application.Exceptions.ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Errors = validationEx.Errors.SelectMany(e => e.Value).ToList()
                }),

            NotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                ApiResponse<object>.FailureResult(notFoundEx.Message)),

            ForbiddenException forbiddenEx => (
                HttpStatusCode.Forbidden,
                ApiResponse<object>.FailureResult(forbiddenEx.Message)),

            BadRequestException badRequestEx => (
                HttpStatusCode.BadRequest,
                ApiResponse<object>.FailureResult(badRequestEx.Message)),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                ApiResponse<object>.FailureResult("Unauthorized.")),

            _ => (
                HttpStatusCode.InternalServerError,
                ApiResponse<object>.FailureResult("An unexpected error occurred."))
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }
        else
        {
            logger.LogWarning(exception, "Handled exception: {Message}", exception.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(json);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
