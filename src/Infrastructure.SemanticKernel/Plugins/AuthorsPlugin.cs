using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public class AuthorResponse : IAuthorResponse
{
    public Guid AuthorId { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? Message { get; set; }
}

public sealed class AuthorsPlugin(IServiceProvider serviceProvider) : IAuthorsPlugin
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [KernelFunction("get_author")]
    [Description("Returns structured author info including name, status, and explanation.")]
    async Task<IAuthorResponse> IAuthorsPlugin.GetAuthorNameAsync(Guid authorId, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();
        var author = await context.Authors.FindAsync([authorId, cancellationToken], cancellationToken: cancellationToken);

        if (author == null)
        {
            return new AuthorResponse
            {
                AuthorId = authorId,
                Name = null,
                Status = "NotFound",
                Message = "No author found with the specified ID."
            };
        }

        return new AuthorResponse
        {
            AuthorId = authorId,
            Name = author.Name,
            Status = string.IsNullOrWhiteSpace(author.Name) ? "Partial" : "Found",
            Message = string.IsNullOrWhiteSpace(author.Name)
                ? "Author exists but name is not yet linked to Entra External ID."
                : "Author found."
        };
    }
}
