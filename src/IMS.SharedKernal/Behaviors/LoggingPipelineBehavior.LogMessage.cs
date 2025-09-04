using IMS.SharedKernal.Results;

using Microsoft.Extensions.Logging;

namespace IMS.ItemInventory.Api.Shared.Behaviors;

// Class used by Compile-time logging source generation to generate the logging messages.
public static partial class LogMessage
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