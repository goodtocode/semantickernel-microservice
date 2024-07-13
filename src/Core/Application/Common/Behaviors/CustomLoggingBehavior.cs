using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Goodtocode.SemanticKernel.Core.Application.Common.Behaviors;

public class CustomLoggingBehavior<TRequest>(ILogger<TRequest> logger) : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger = logger;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        await Task.Run(() => _logger.LogInformation("Request: {Name} {@Request}",
            requestName, request), cancellationToken);
    }
}