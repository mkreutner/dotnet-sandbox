// File: Middlewares/GlobalExceptionHandler.cs
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using FluentValidation; // <-- Ajoute absolument ce using !

namespace Todo.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Une exception non gérée s'est produite : {Message}", exception.Message);

        int statusCode = StatusCodes.Status500InternalServerError;
        object errorResponse = new { Error = "An unexpected error occurred." };

        // Pattern Matching sur l'exception
        switch (exception) 
        {
          // On ajoute le cas FluentValidation !
          case ValidationException validationException:
            statusCode = (int)HttpStatusCode.BadRequest; // Code 400
            errorResponse = new 
            {
                Message = "Validation failed",
                Errors = validationException.Errors.Select(e => new 
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                })
            };
            break;

          case ArgumentException:
            statusCode = (int)HttpStatusCode.BadRequest;
            errorResponse = new { Error = exception.Message };
            break;

          case KeyNotFoundException:
            statusCode = (int)HttpStatusCode.NotFound;
            errorResponse = new { Error = exception.Message };
            break;
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        // On utilise WriteAsJsonAsync avec notre objet (qu'il soit anonyme ou simple texte)
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true; 
    }
}
