using Goodtocode.Domain.Types;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatMessageEntity : DomainEntity<ChatMessageEntity>
{
    public Guid ChatSessionId { get; set; } = Guid.Empty;
    public ChatMessageRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public virtual ChatSessionEntity ChatSession { get; set; } = new();
}