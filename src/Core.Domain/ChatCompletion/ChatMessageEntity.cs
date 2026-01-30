using Goodtocode.Domain.Entities;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatMessageEntity : DomainEntity<ChatMessageEntity>, IDomainEntity<ChatMessageEntity>
{
    protected ChatMessageEntity() { }
    public Guid ChatSessionId { get; private set; }
    public ChatMessageRole Role { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public virtual ChatSessionEntity? ChatSession { get; private set; }
    public static ChatMessageEntity Create(Guid id, Guid chatSessionId, ChatMessageRole role, string content)
    {
        return new ChatMessageEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            ChatSessionId = chatSessionId,
            Role = role,
            Content = content,
            Timestamp = DateTime.UtcNow
        };
    }
}
