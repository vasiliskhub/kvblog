using Kvblog.Api.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kvblog.Api.Extensions;

public static class ResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(result.ErrorType),
            Status = GetStatusCode(result.ErrorType),
            Detail = result.Error
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new NoContentResult();
        }

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(result.ErrorType),
            Status = GetStatusCode(result.ErrorType),
            Detail = result.Error
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    public static IActionResult ToCreatedResult<T>(this Result<T> result, HttpRequest request, string resourcePath)
    {
        if (result.IsSuccess)
        {
            var locationUri = $"{request.Scheme}://{request.Host}{resourcePath}/{result.Value}";
            return new CreatedResult(locationUri, result.Value);
        }

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(result.ErrorType),
            Status = GetStatusCode(result.ErrorType),
            Detail = result.Error
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private static string GetTitle(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.NotFound => "Not Found",
            ErrorType.Validation => "Validation Error",
            ErrorType.General => "An error occurred",
            _ => "An error occurred"
        };
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.General => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
