using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public sealed class AuthorsPlugin : IAuthorsPlugin
{
    private readonly ISemanticKernelContext _context;

    public AuthorsPlugin(ISemanticKernelContext context) => _context = context;

    [KernelFunction("get_author")]
    [Description("Retrieves detailed information about an author based on their ID.")]
    public async Task<string> GetAuthorInfoAsync(string authorId, CancellationToken cancellationToken)
    {
        var author = await _context.Authors.FindAsync([authorId, cancellationToken], cancellationToken: cancellationToken);
        return author?.Name ?? "Author not found";
    }
}
