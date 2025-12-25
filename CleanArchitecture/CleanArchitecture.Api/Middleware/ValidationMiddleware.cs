using FluentValidation;

namespace CleanArchitecture.Api.Middleware;

/// <summary>
/// Middleware to validate requests using FluentValidation
/// </summary>
public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
    }
}

/// <summary>
/// Filter to automatically validate requests using FluentValidation
/// </summary>
public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T>? _validator;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _validator = serviceProvider.GetService<IValidator<T>>();
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (_validator != null)
        {
            var model = context.Arguments.OfType<T>().FirstOrDefault();
            if (model != null)
            {
                var validationResult = await _validator.ValidateAsync(model);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
            }
        }

        return await next(context);
    }
}
