using Goodtocode.Domain.Types.DomainEntity;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatMessageEntity : DomainEntity<ChatMessageEntity>, IDomainEntity<ChatMessageEntity>
{
    private ChatMessageEntity() { }
    public Guid ChatSessionId { get; set; }
    public ChatMessageRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public virtual ChatSessionEntity? ChatSession { get; set; }
    public static ChatMessageEntity Create(Guid id, Guid chatSessionId, ChatMessageRole role, string content)
    {
        return new ChatMessageEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            ChatSessionId = chatSessionId,
            Role = role,
            Content = content
        };
    }
}
