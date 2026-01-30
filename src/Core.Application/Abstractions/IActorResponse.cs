namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IActorResponse
{
    Guid ActorId { get; }
    string? Name { get; }
    string Status { get; }
    string? Message { get; }
}