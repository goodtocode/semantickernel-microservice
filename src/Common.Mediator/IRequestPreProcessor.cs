namespace Goodtocode.Mediator;

public interface IRequestPreProcessor<TRequest> where TRequest : notnull
{
    Task Process(TRequest request, CancellationToken cancellationToken);
}
