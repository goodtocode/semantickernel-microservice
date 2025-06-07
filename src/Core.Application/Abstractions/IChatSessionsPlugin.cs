namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Plugins;

public interface IChatSessionsPlugin
{
    Task<IEnumerable<string>> ListRecentSessionsAsync(DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken);
    Task<string> UpdateChatSessionTitleAsync(string sessionId, string newTitle, CancellationToken cancellationToken);
}