namespace Movies.Api.Mapping;

using FluentValidation;
using Movies.Contracts.Responses;

public class ValidationMappingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMappingMiddleware(RequestDelegate next)
    {
        _next = next;   
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException exception)
        {
            httpContext.Response.StatusCode = 400;
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = exception.Errors.Select(e => new ValidationResponse
                {
                    PropertyName = e.PropertyName,
                    Message = e.ErrorMessage
                })
            };
            
            await httpContext.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}