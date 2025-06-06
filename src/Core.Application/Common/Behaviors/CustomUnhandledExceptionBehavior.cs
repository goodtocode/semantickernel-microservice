using Microsoft.Extensions.Logging;

namespace Goodtocode.SemanticKernel.Core.Application.Common.Behaviors;

public class CustomUnhandledExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            await Task.Run(()
                => logger.LogError(ex, "Request: Unhandled Exception for Request {Name} {@Request}", requestName, request), cancellationToken);

            throw;
        }
    }
}
