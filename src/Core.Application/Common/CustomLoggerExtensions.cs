using Microsoft.Extensions.Logging;

namespace Goodtocode.SemanticKernel.Core.Application.Common
{
    public static partial class CustomLoggingBehaviorExtensions
    {
        [LoggerMessage(EventId = 100, Level = LogLevel.Information, Message = "Request: {Name}")]
        public static partial void LogRequest(this ILogger logger, string name);

        [LoggerMessage(EventId = 101, Level = LogLevel.Warning, Message = "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds)")]
        public static partial void LogLongRunningRequest(this ILogger logger, string name, long elapsedMilliseconds);

        [LoggerMessage(EventId = 102, Level = LogLevel.Error, Message = "Request: Unhandled Exception for Request {Name}")]
        public static partial void LogUnhandledException(this ILogger logger, Exception exception, string name);
    }
}
