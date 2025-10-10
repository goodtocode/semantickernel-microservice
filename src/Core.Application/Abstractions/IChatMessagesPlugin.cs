namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IChatMessagesPlugin : ISemanticPluginCompatible
{
    Task<IEnumerable<string>> ListRecentMessagesAsync(DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetChatMessagesAsync(Guid sessionId, CancellationToken cancellationToken);
}
