using Microsoft.Extensions.Logging;

namespace Goodtocode.SemanticKernel.Core.Application.Common.Behaviors;

public class CustomUnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public CustomUnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
