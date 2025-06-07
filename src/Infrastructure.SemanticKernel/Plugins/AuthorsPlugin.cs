using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public sealed class AuthorsPlugin : IAuthorsPlugin
{
    private readonly IServiceProvider _serviceProvider;

    public AuthorsPlugin(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    [KernelFunction("get_author")]
    [Description("Retrieves detailed information about an author based on their ID.")]
    public async Task<string> GetAuthorInfoAsync(string authorId, CancellationToken cancellationToken)
    {
        // Get ISemanticKernelContext directly instead of constructor DI to allow this plugin to be registered via AddSingleton() and not scoped due to EF.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();
        var author = await context.Authors.FindAsync([authorId, cancellationToken], cancellationToken: cancellationToken);
        return author?.Name ?? "Author not found";
    }
}
