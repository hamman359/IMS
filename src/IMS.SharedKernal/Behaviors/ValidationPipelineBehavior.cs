using FluentValidation;

using IMS.SharedKernal.Results;

using MediatR;

namespace IMS.SharedKernal.Behaviors;

/// <summary>
/// Defines a MediatR pipeline behavior for handling the Validation of request data
/// </summary>
public class ValidationPipelineBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest
    where TResponse : Result
{
    /// <summary>
    /// Validates the request.
    /// Ifany errors, returns validation result.
    /// Otherwise, returns the result of the next() delegate execution.
    /// Skips the validation if there are not any validators defined.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // If there aren't any validators to check we can just forward to the next call
        if (!validators.Any())
        {
            return await next();
        }

        // Check all of the validators to see if there are any errors
        Error[] errors = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(
                failure.PropertyName,
                failure.ErrorMessage))
            .Distinct()
            .ToArray();

        // If there are any errors, we want to convert them to Validation Results
        // and return them WITHOUT forwarding the call to anything downstream.
        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        // If there were no errors we can forward the call to the next operation
        return await next();
    }

    // Helper method to convert the Errors to Validation Results
    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        var validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)validationResult;
    }
}