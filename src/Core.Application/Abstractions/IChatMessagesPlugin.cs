namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public interface IChatMessagesPlugin
{
    Task<IEnumerable<string>> ListRecentMessagesAsync(DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetChatMessagesAsync(string sessionId, CancellationToken cancellationToken);
}
