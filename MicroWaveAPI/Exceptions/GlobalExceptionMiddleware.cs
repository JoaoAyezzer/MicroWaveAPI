using System.Net;

namespace MicroWaveAPI.Exceptions;

using System;
public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        string message;
        
        switch (exception)
        {
            case ObjectNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "Ocorreu um erro inesperado.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}