
using IMS.ItemInventory.Api.Shared.Results;

using MediatR;

namespace IMS.ItemInventory.Api.Shared.Behaviors;

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
        LoggerMessages.LogStartingRequest(logger, typeof(TRequest).Name, DateTime.UtcNow);

        var result = await next();

        if (result.IsFailure)
        {
            LoggerMessages.LogRequestFailure(
                logger,
                typeof(TRequest).Name,
                result.Errors,
                DateTime.UtcNow);

        }

        LoggerMessages.LogCompletedRequest(logger, typeof(TRequest).Name, DateTime.UtcNow);

        return result;
    }

}

public static partial class LoggerMessages
{
    [LoggerMessage(
        EventId = 0,
        EventName = "Start Request",
        Level = LogLevel.Information,
        Message = "Starting request {@RequestName}, {@DateTimeUtc}")]
    public static partial void LogStartingRequest(
        ILogger logger,
        string RequestName,
        DateTime DateTimeUtc);


    [LoggerMessage(
        EventId = 0,
        EventName = "Start Completed",
        Level = LogLevel.Information,
        Message = "Completed request {@RequestName}, {@DateTimeUtc}")]
    public static partial void LogCompletedRequest(
        ILogger logger,
        string RequestName,
        DateTime DateTimeUtc);

    [LoggerMessage(
        EventId = 0,
        EventName = "Request failure",
        Level = LogLevel.Error,
        Message = "Request failure {@RequestName}, {@Errors}, {@DateTimeUtc}")]
    public static partial void LogRequestFailure(
        ILogger logger,
        string RequestName,
        Error[] Errors,
        DateTime DateTimeUtc);
}