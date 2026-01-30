using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public class ActorResponse : IActorResponse
{
    public Guid ActorId { get; set; }
    public string? Name { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Message { get; set; }
}


public sealed class ActorsPlugin(IServiceProvider serviceProvider) : IActorsPlugin
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public string PluginName => "AuthorsPlugin";
    public string FunctionName => _currentFunctionName;
    public Dictionary<string, object> Parameters => _currentParameters;

    private string _currentFunctionName = string.Empty;
    private Dictionary<string, object> _currentParameters = [];

    [KernelFunction("get_actor_by_id")]
    [Description("Returns structured actor info by ID including name, status, and explanation.")]
    public async Task<IActorResponse> GetActorByIdAsync(Guid actorId, CancellationToken cancellationToken)
    {
        _currentFunctionName = "get_actor_by_id";
        _currentParameters = new()
        {
            { "actorId", actorId }
        };

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();
        var actor = await context.Actors.FindAsync([actorId, cancellationToken], cancellationToken: cancellationToken);

        if (actor == null)
        {
            return new ActorResponse
            {
                ActorId = actorId,
                Name = null,
                Status = "NotFound",
                Message = "No actor found with the specified ID."
            };
        }

        return new ActorResponse
        {
            ActorId = actorId,
            Name = $"{actor.FirstName} {actor.LastName}",
            Status = string.IsNullOrWhiteSpace($"{actor.FirstName} {actor.LastName}") ? "Partial" : "Found",
            Message = string.IsNullOrWhiteSpace($"{actor.FirstName} {actor.LastName}")
                ? "Actor exists but name is not yet linked to Entra External ID."
                : "Actor found."
        };
    }

    [KernelFunction("get_actors_by_name")]
    [Description("Returns structured actor info by name including ID, status, and explanation.")]
    public async Task<ICollection<IActorResponse>> GetActorsByNameAsync(string name, CancellationToken cancellationToken)
    {
        _currentFunctionName = "get_actors_by_name";
        _currentParameters = new()
        {
            { "name", name }
        };

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();
        var nameTokens = name?.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
        var normalizedInput = name?.Trim() ?? string.Empty;

        var authors = await context.Actors
            .Where(a =>
                nameTokens.Any(token =>
                    EF.Functions.Like(a.FirstName, $"%{token}%") ||
                    EF.Functions.Like(a.LastName, $"%{token}%")
                )
                || EF.Functions.Like(
                    (a.FirstName + " " + a.LastName).Trim(), $"%{normalizedInput}%"
                )
                || EF.Functions.Like(
                    (a.LastName + " " + a.FirstName).Trim(), $"%{normalizedInput}%"
                )
                || nameTokens.Any(token =>
                    EF.Functions.Like(a.FirstName, $"{token}%") ||
                    EF.Functions.Like(a.FirstName, $"%{token}") ||
                    EF.Functions.Like(a.LastName, $"{token}%") ||
                    EF.Functions.Like(a.LastName, $"%{token}")
                )
            )
            .ToListAsync(cancellationToken);

        if (authors.Count == 0)
        {
            return [ new ActorResponse
            {
                ActorId = Guid.Empty,
                Name = name,
                Status = "NotFound",
                Message = "No actor found with the specified name."
            } ];
        }
        else
        {
            return [.. authors.Select(a => new ActorResponse
            {
                ActorId = a.Id,
                Name = $"{a.FirstName} {a.LastName}",
                Status = string.IsNullOrWhiteSpace($"{a.FirstName} {a.LastName}") ? "Partial" : "Found",
                Message = string.IsNullOrWhiteSpace($"{a.FirstName} {a.LastName}")
                    ? "Actor exists but name is not yet linked to Entra External ID."
                    : "Actor found."
            })];
        }
    }
}
