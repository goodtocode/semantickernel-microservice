using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public sealed class ChatMessagesPlugin(IServiceProvider serviceProvider)
    : IChatMessagesPlugin, ISemanticPluginCompatible
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public string PluginName => "ChatMessagesPlugin";
    public string FunctionName => _currentFunctionName;
    public Dictionary<string, object> Parameters => _currentParameters;

    private string _currentFunctionName = string.Empty;
    private Dictionary<string, object> _currentParameters = [];

    [KernelFunction("list_messages")]
    [Description("Retrieves the most recent messages from all chat sessions.")]
    public async Task<IEnumerable<string>> ListRecentMessagesAsync(DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        _currentFunctionName = "list_messages";
        _currentParameters = new()
    {
        { "startDate", startDate ?? DateTime.UtcNow.AddDays(-7) },
        { "endDate", endDate  ?? DateTime.UtcNow.AddSeconds(1)}
    };

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();

        var query = context.ChatMessages.AsQueryable();
        if (startDate.HasValue) query = query.Where(x => x.Timestamp >= startDate.Value);
        if (endDate.HasValue) query = query.Where(x => x.Timestamp <= endDate.Value);

        var messages = await query.OrderByDescending(x => x.Timestamp).ToListAsync(cancellationToken);
        return messages.Select(m => $"{m.ChatSessionId}: {m.Timestamp:u} - {m.Role}: {m.Content}");
    }

    [KernelFunction("get_messages")]
    [Description("Retrieves all messages from a specific chat session.")]
    public async Task<IEnumerable<string>> GetChatMessagesAsync(Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        _currentFunctionName = "get_messages";
        _currentParameters = new()
    {
        { "sessionId", sessionId }
    };

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();

        var messages = await context.ChatMessages
            .Where(x => x.ChatSessionId == sessionId)
            .ToListAsync(cancellationToken);

        return messages.Select(m => $"{m.ChatSessionId}: {m.Timestamp:u} - {m.Role}: {m.Content}");
    }
}