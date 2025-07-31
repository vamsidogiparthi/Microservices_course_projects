using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("An unhandled exception occurred: {ExceptionMessage}, Time of Occurance {Time}", exception.Message, DateTime.UtcNow);
        
        (string detail, string title, int statusCode) = exception switch
        {
            InternalServerException internalServerException => (internalServerException.Details, internalServerException.GetType().Name, StatusCodes.Status500InternalServerError),
            ValidationException validationException => (validationException.Message, validationException.GetType().Name, StatusCodes.Status400BadRequest),
            BadRequestException badRequestException => (badRequestException.Message, badRequestException.GetType().Name, StatusCodes.Status400BadRequest),
            KeyNotFoundException keyNotFoundException => (keyNotFoundException.Message, keyNotFoundException.GetType().Name, StatusCodes.Status404NotFound),
            NotFoundException notFoundException => (notFoundException.Message, notFoundException.GetType().Name, StatusCodes.Status404NotFound),

            _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError)
        };

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
        problemDetails.Extensions.Add("timestamp", DateTime.UtcNow.ToString("o"));

        if(exception is ValidationException validationException1)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException1.Errors);
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}
