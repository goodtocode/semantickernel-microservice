namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IAuthorsPlugin
{
    Task<IAuthorResponse> GetAuthorNameAsync(Guid authorId, CancellationToken cancellationToken);
}