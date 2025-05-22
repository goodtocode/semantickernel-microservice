using Goodtocode.Domain.Types;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatMessageEntity : DomainEntity<ChatMessageEntity>, IDomainEntity<ChatMessageEntity>
{
    public Guid ChatSessionId { get; set; }
    public ChatMessageRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public virtual ChatSessionEntity ChatSession { get; set; } = new();
}
