namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IChatSessionsPlugin : ISemanticPluginCompatible
{
    Task<IEnumerable<string>> ListRecentSessionsAsync(DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken);
    Task<string> UpdateChatSessionTitleAsync(Guid sessionId, string newTitle, CancellationToken cancellationToken);
}