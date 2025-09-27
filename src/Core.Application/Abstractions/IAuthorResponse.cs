namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IAuthorResponse
{
    Guid AuthorId { get; }
    string? Name { get; }
    string Status { get; }
    string? Message { get; }
}