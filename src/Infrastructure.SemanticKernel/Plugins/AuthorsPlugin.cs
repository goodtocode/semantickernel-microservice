using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public sealed class AuthorsPlugin(IServiceProvider serviceProvider) : IAuthorsPlugin
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [KernelFunction("get_author")]
    [Description("Returns the author's name for the specified author ID, or 'Author not found' if no match exists.")]
    public async Task<string> GetAuthorInfoAsync(string authorId, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();
        var author = await context.Authors.FindAsync([authorId, cancellationToken], cancellationToken: cancellationToken);
        return author?.Name ?? "Author not found";
    }
}
