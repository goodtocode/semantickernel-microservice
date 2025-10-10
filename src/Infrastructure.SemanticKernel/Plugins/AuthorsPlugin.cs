using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
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

    public string PluginName => "AuthorsPlugin";
    public string FunctionName => _currentFunctionName;
    public Dictionary<string, object> Parameters => _currentParameters;

    private string _currentFunctionName = string.Empty;
    private Dictionary<string, object> _currentParameters = new();

    [KernelFunction("get_author_by_id")]
    [Description("Returns structured author info by ID including name, status, and explanation.")]
    public async Task<IAuthorResponse> GetAuthorByIdAsync(Guid authorId, CancellationToken cancellationToken)
    {
        _currentFunctionName = "get_author_by_id";
        _currentParameters = new()
        {
            { "authorId", authorId }
        };

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

    [KernelFunction("get_authors_by_name")]
    [Description("Returns structured author info by name including ID, status, and explanation.")]
    public async Task<ICollection<IAuthorResponse>> GetAuthorsByNameAsync(string name, CancellationToken cancellationToken)
    {
        _currentFunctionName = "get_authors_by_name";
        _currentParameters = new()
        {
            { "name", name }
        };

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();
        var authors = await context.Authors
            .Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{name}%"))
            .ToListAsync(cancellationToken);

        if (authors.Count == 0)
        {
            return [ new AuthorResponse
            {
                AuthorId = Guid.Empty,
                Name = name,
                Status = "NotFound",
                Message = "No author found with the specified name."
            } ];
        }
        else
        {
            return [.. authors.Select(a => new AuthorResponse
            {
                AuthorId = a.Id,
                Name = a.Name,
                Status = string.IsNullOrWhiteSpace(a.Name) ? "Partial" : "Found",
                Message = string.IsNullOrWhiteSpace(a.Name)
                    ? "Author exists but name is not yet linked to Entra External ID."
                    : "Author found."
            })];
        }
    }
}
