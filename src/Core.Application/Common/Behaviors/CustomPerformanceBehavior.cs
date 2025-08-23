using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Goodtocode.SemanticKernel.Core.Application.Common.Behaviors;

public class CustomPerformanceBehavior<TRequest, TResponse>(
    ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer = new();
    private readonly ILogger<TRequest> _logger = logger;

    public Task<TResponse> Handle(TRequest request, RequestDelegateInvoker<TResponse> nextInvoker, CancellationToken cancellationToken)
    {
        return Handle(request, nextInvoker, _logger, cancellationToken);
    }

    public async Task<TResponse> Handle(TRequest request, RequestDelegateInvoker<TResponse> nextInvoker, ILogger logger, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await nextInvoker();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            await Task.Run(() =>
                logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", requestName, elapsedMilliseconds, request), cancellationToken);
        }

        return response;
    }
}

public class CustomPerformanceBehavior<TRequest>(
    ILogger<TRequest> logger) : IPipelineBehavior<TRequest> where TRequest : notnull
{
    private readonly Stopwatch _timer = new();
    private readonly ILogger<TRequest> _logger = logger;

    public Task Handle(TRequest request, RequestDelegateInvoker nextInvoker, CancellationToken cancellationToken)
    {
        return Handle(request, nextInvoker, _logger, cancellationToken);
    }

    public async Task Handle(TRequest request, RequestDelegateInvoker nextInvoker, ILogger logger, CancellationToken cancellationToken)
    {
        _timer.Start();

        await nextInvoker();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            await Task.Run(() =>
                logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", requestName, elapsedMilliseconds, request), cancellationToken);
        }
    }
}
