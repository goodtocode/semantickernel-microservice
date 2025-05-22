using Goodtocode.Domain.Types;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatMessageEntity : DomainEntity<ChatMessageEntity>, IDomainEntity<ChatMessageEntity>
{
    public Guid ChatSessionId { get; set; }
    public ChatMessageRole Role { get; set; }
    public string Content { get; set; }
    public virtual ChatSessionEntity ChatSession { get; set; }
    public List<ChatMessageEntity> Messages { get; set; } = [];
}
