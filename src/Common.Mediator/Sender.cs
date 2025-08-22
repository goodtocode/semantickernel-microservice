namespace Goodtocode.Mediator;

public class Sender(IRequestDispatcher dispatcher) : ISender
{
    public Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        return dispatcher.Send(request, cancellationToken);
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return dispatcher.Send(request, cancellationToken);
    }
}