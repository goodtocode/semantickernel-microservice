namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IAuthorsPlugin : ISemanticPluginCompatible
{
    Task<IAuthorResponse> GetAuthorByIdAsync(Guid authorId, CancellationToken cancellationToken);
    Task<ICollection<IAuthorResponse>> GetAuthorsByNameAsync(string name, CancellationToken cancellationToken);
}