// File: Middlewares/GlobalExceptionHandler.cs
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

public class GlobalExceptionHandler : IExceptionHandler
{
    // You can inject a logger to write the error to the Linux/Docker console.
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
        // 1. Log the error for the sysadmin (you!)
        _logger.LogError(exception, "Une exception non gérée s'est produite : {Message}", exception.Message);

        // 2. Prepare the default HTTP response (Error 500)
        int statusCode = StatusCodes.Status500InternalServerError;
        string errorMessage = "An unexpected error occurred.";

        // 3. Pattern Matching! 
        // This is where we sort the exception by type to adjust the HTTP code.
        switch (exception) 
        {
          case ArgumentException:
            statusCode = (int)HttpStatusCode.BadRequest;
            errorMessage = exception.Message;
            break;
          case KeyNotFoundException:
            statusCode = (int)HttpStatusCode.NotFound;
            errorMessage = exception.Message;
            break;
        }

        // 4. Configure the final HTTP response
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        // 5. Send a nice, standardized JSON response to the client
        var response = new { Error = errorMessage };
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        // Signal to .NET that the exception has been successfully handled (it stops here)
        return true; 
    }
}
