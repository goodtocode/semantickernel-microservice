using Microsoft.Extensions.DependencyInjection;

namespace Goodtocode.Mediator;


public class RequestDispatcher(IServiceProvider serviceProvider) : IRequestDispatcher
{
    public async Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
        var handler = serviceProvider.GetRequiredService(handlerType);

        var behaviorType = typeof(IPipelineBehavior<>).MakeGenericType(requestType);
        var behaviors = serviceProvider.GetServices(behaviorType)?.ToList() ?? [];

        RequestDelegateInvoker handlerDelegate = () =>
            ((dynamic)handler).Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () => ((dynamic)behavior)?.Handle((dynamic)request, next, cancellationToken)
                ?? throw new InvalidOperationException("Pipeline behavior is null.");
        }

        await handlerDelegate();
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = serviceProvider.GetServices(behaviorType)?.ToList() ?? [];

        RequestDelegateInvoker<TResponse> handlerDelegate = () =>
            ((dynamic)handler).Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () => ((dynamic)behavior)?.Handle((dynamic)request, next, cancellationToken)
                ?? throw new InvalidOperationException("Pipeline behavior is null.");
        }

        return await handlerDelegate();
    }
}
