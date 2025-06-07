using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public sealed class ChatSessionsPlugin : IChatSessionsPlugin
{
    private readonly ISemanticKernelContext _context;

    public ChatSessionsPlugin(ISemanticKernelContext context) => _context = context;

    [KernelFunction("list_sessions")]
    [Description("Lists the latest chat sessions. Optionally filter by start and/or end date.")]
    public async Task<IEnumerable<string>> ListRecentSessionsAsync(DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ChatSessions.AsQueryable();

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
    public async Task<string> UpdateChatSessionTitleAsync(string sessionId, string newTitle,
        CancellationToken cancellationToken = default)
    {
        var chatSession = await _context.ChatSessions
            .FirstOrDefaultAsync(x => x.Id.ToString() == sessionId, cancellationToken: cancellationToken);
        chatSession!.Title = newTitle;
        _context.ChatSessions.Update(chatSession);
        await _context.SaveChangesAsync(cancellationToken);

        return $"{chatSession.Id}: {chatSession.Timestamp} - {chatSession.Title}: {chatSession.Author?.Name}";
    }
}
