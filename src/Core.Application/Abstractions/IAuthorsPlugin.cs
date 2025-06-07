namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public interface IAuthorsPlugin
{
    Task<string> GetAuthorInfoAsync(string authorId, CancellationToken cancellationToken);
}