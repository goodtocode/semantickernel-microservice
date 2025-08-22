namespace Goodtocode.Mediator;

public delegate Task RequestDelegateInvoker();
public interface IPipelineBehavior<TRequest> where TRequest : notnull
{
    Task Handle(TRequest request, RequestDelegateInvoker nextInvoker, CancellationToken cancellationToken);
}

public delegate Task<TResponse> RequestDelegateInvoker<TResponse>();
public interface IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    Task<TResponse> Handle(TRequest request, RequestDelegateInvoker<TResponse> nextInvoker, CancellationToken cancellationToken);
}

