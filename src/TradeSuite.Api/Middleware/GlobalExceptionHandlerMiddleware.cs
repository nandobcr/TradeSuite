using Microsoft.AspNetCore.Mvc;

namespace TradeSuite.Api.Middleware;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status400BadRequest);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(context, ex, ex.Message, StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex,
                "An unexpected error occurred. Please contact support with the traceId.",
                StatusCodes.Status500InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception ex,
        string message,
        int statusCode)
    {
        /*if (logger.IsEnabled(LogLevel.Error))
        {
            logger.LogError(ex, "Unexpected error occurred. TraceId: {TraceId}", context.TraceIdentifier);
        }*/

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = message,
            Extensions =
            {
                ["traceId"] = context.TraceIdentifier
            }
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}