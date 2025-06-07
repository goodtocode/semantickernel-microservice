using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public sealed class ChatMessagesPlugin : IChatMessagesPlugin
{
    private readonly IServiceProvider _serviceProvider;

    public ChatMessagesPlugin(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    [KernelFunction("list_messages")]
    [Description("Lists the latest messages across all chat sessions. Optionally filter by start and/or end date.")]
    public async Task<IEnumerable<string>> ListRecentMessagesAsync(DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        // Get ISemanticKernelContext directly instead of constructor DI to allow this plugin to be registered via AddSingleton() and not scoped due to EF.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();

        var query = context.ChatMessages.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(x => x.Timestamp > startDate.Value);
        if (endDate.HasValue)
            query = query.Where(x => x.Timestamp < endDate.Value);

        var messages = await query
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync(cancellationToken);

        return messages.Select(m => $"{m.ChatSessionId}: {m.Timestamp} - {m.Role}: {m.Content}");
    }

    [KernelFunction("get_messages")]
    [Description("Retrieves all messages from a specific chat session.")]
    public async Task<IEnumerable<string>> GetChatMessagesAsync(string sessionId,
        CancellationToken cancellationToken = default)
    {
        // Get ISemanticKernelContext directly instead of constructor DI to allow this plugin to be registered via AddSingleton() and not scoped due to EF.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ISemanticKernelContext>();

        var messages = await context.ChatMessages
        .Where(x => x.ChatSessionId.ToString() == sessionId)
            .ToListAsync(cancellationToken);

        return messages.Select(m => $"{m.ChatSessionId}: {m.Timestamp} - {m.Role}: {m.Content}");
    }
}
