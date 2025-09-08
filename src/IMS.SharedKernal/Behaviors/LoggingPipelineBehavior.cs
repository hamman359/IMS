using IMS.ItemInventory.Api.Shared.Behaviors;

using Microsoft.Extensions.Logging;

namespace IMS.SharedKernal.Behaviors;

/// <summary>
/// Defines a MediatR pipeline behavior for logging requests that come through MediatR.
/// Has Type Constraints to ensure TRequest is an IRequest<> and that TResponse is a Result.
/// </summary>
public class LoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    /// <summary>
    /// Logs when a request started and when it finished.
    /// If the request returned a failure response this is also logged.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Using Compile-time logging source generation to make log messages
        // Easier to read and more performant
        LogMessage.LogStartingRequest(logger, typeof(TRequest).Name, DateTime.UtcNow);

        var result = await next();

        if (result.IsFailure)
        {
            LogMessage.LogRequestFailure(
                logger,
                typeof(TRequest).Name,
                result.Errors,
                DateTime.UtcNow);

        }

        LogMessage.LogCompletedRequest(logger, typeof(TRequest).Name, DateTime.UtcNow);

        return result;
    }

}
