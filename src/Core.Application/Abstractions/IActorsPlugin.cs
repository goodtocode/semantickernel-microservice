namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IActorsPlugin : ISemanticPluginCompatible
{
    Task<IActorResponse> GetActorByIdAsync(Guid actorId, CancellationToken cancellationToken);
    Task<ICollection<IActorResponse>> GetActorsByNameAsync(string name, CancellationToken cancellationToken);
}