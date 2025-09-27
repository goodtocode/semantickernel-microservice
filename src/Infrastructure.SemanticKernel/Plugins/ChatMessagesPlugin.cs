using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public sealed class ChatMessagesPlugin(IServiceProvider serviceProvider) : IChatMessagesPlugin
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [KernelFunction("list_messages")]
    [Description("Retrieves the most recent messages from all chat sessions, optionally filtered by a start and/or end date. Results are ordered from newest to oldest.")]
    public async Task<IEnumerable<string>> ListRecentMessagesAsync(DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        // Resolve ISemanticKernelContext from a new scope to ensure correct EF Core lifetime management.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();

        var query = context.ChatMessages.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.Timestamp >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(x => x.Timestamp <= endDate.Value);

        var messages = await query
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);

        return messages.Select(m => $"{m.ChatSessionId}: {m.Timestamp:u} - {m.Role}: {m.Content}");
    }

    [KernelFunction("get_messages")]
    [Description("Retrieves all messages from a specific chat session.")]
    public async Task<IEnumerable<string>> GetChatMessagesAsync(Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        // Get ISemanticKernelContext directly instead of constructor DI to allow this plugin to be registered via AddSingleton() and not scoped due to EF.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();

        var messages = await context.ChatMessages
        .Where(x => x.ChatSessionId == sessionId)
            .ToListAsync(cancellationToken);

        return messages.Select(m => $"{m.ChatSessionId}: {m.Timestamp} - {m.Role}: {m.Content}");
    }
}
