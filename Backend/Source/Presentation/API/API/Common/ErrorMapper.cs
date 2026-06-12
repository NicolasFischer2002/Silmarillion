using Microsoft.AspNetCore.Mvc;
using SharedKernel.Results;

namespace API.Common
{
    public static class ErrorMapper
    {
        public static IResult Map(List<Error> errors)
        {
            var first = errors.First();

            var statusCode = first.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.BusinessRule => StatusCodes.Status400BadRequest,
                ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status400BadRequest
            };

            var problem = new ProblemDetails
            {
                Title = first.Type.ToString(),
                Detail = string.Join("; ", errors.Select(e => e.Message)),
                Status = statusCode,
                Extensions =
            {
                ["errors"] = errors.Select(e => new
                {
                    e.Code,
                    e.Message,
                    e.Type
                })
            }
            };

            return Results.Problem(problem);
        }
    }
}