using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Goodtocode.SemanticKernel.Core.Application.Common.Behaviors;

public class CustomPerformanceBehavior<TRequest, TResponse>(
    ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer = new();
    private readonly ILogger<TRequest> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestDelegateInvoker<TResponse> nextInvoker, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await nextInvoker();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            await Task.Run(() => _logger.LogLongRunningRequest(requestName, elapsedMilliseconds), cancellationToken);
        }

        return response;
    }
}

public class CustomPerformanceBehavior<TRequest>(
    ILogger<TRequest> logger) : IPipelineBehavior<TRequest> where TRequest : notnull
{
    private readonly Stopwatch _timer = new();
    private readonly ILogger<TRequest> _logger = logger;

    public async Task Handle(TRequest request, RequestDelegateInvoker nextInvoker, CancellationToken cancellationToken)
    {
        _timer.Start();

        await nextInvoker();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            await Task.Run(() => _logger.LogLongRunningRequest(requestName, elapsedMilliseconds), cancellationToken);
        }
    }
}