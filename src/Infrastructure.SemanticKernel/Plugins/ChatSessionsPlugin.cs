using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public interface ISemanticPluginCompatible
{
    string PluginName { get; }
    string FunctionName { get; }
    Dictionary<string, object> Parameters { get; }
}

public sealed class ChatSessionsPlugin(IServiceProvider serviceProvider) : IChatSessionsPlugin, ISemanticPluginCompatible
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public string PluginName => "ChatSessionsPlugin";
    public string FunctionName => _currentFunctionName;
    public Dictionary<string, object> Parameters => _currentParameters;

    private string _currentFunctionName = string.Empty;
    private Dictionary<string, object> _currentParameters = new();

    [KernelFunction("list_sessions")]
    [Description("Retrieves a list of recent chat sessions. Optionally, filter results by start and/or end date to narrow the search.")]
    public async Task<IEnumerable<string>> ListRecentSessionsAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        _currentFunctionName = "list_sessions";
        _currentParameters = new()
        {
            { "startDate", startDate ?? DateTime.UtcNow.AddDays(-7) },
            { "endDate", endDate ?? DateTime.UtcNow.AddSeconds(1)}
        };

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();

        var query = context.ChatSessions.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.Timestamp > startDate.Value);
        if (endDate.HasValue)
            query = query.Where(x => x.Timestamp < endDate.Value);

        var messages = await query
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);

        return messages.Select(m => $"{m.Id}: {m.Timestamp} - {m.Title}");
    }

    [KernelFunction("change_title")]
    [Description("Changes the title on this chat session.")]
    public async Task<string> UpdateChatSessionTitleAsync(Guid sessionId, string newTitle, CancellationToken cancellationToken = default)
    {
        _currentFunctionName = "change_title";
        _currentParameters = new()
        {
            { "sessionId", sessionId },
            { "newTitle", newTitle }
        };

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();

        var chatSession = await context.ChatSessions
            .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken: cancellationToken);

        if (chatSession == null)
        {
            return $"Session {sessionId} not found.";
        }

        chatSession.Title = newTitle;
        context.ChatSessions.Update(chatSession);
        await context.SaveChangesAsync(cancellationToken);

        return $"{chatSession.Id}: {chatSession.Timestamp} - {chatSession.Title}: {chatSession.Author?.Name}";
    }
}