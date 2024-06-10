using FluentValidation;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping;

public class ValidationMappingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400;
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = exception.Errors.Select(failure => new ValidationResponse
                {
                    PropertyName = failure.PropertyName,
                    Message = failure.ErrorMessage
                })
            };

            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}
