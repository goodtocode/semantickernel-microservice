using Goodtocode.Domain.Entities;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatSessionEntity : DomainEntity<ChatSessionEntity>
{
    protected ChatSessionEntity() { }

    public Guid ActorId { get; private set; }
    public string? Title { get; private set; } = string.Empty;
    public virtual ICollection<ChatMessageEntity> Messages { get; private set; } = [];
    public virtual ActorEntity? Actor { get; private set; }

    public static ChatSessionEntity Create(Guid id, Guid authorId, string? title, ChatMessageRole responseRole, string initialMessage, string responseMessage)
    {
        var session = new ChatSessionEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            ActorId = authorId,
            Title = title,
            Timestamp = DateTime.UtcNow
        };
        session.Messages.Add(ChatMessageEntity.Create(Guid.NewGuid(), session.Id, ChatMessageRole.user, initialMessage));
        session.Messages.Add(ChatMessageEntity.Create(Guid.NewGuid(), session.Id, responseRole, responseMessage));
        return session;
    }

    public void Update(string? title)
    {
            Title = title ?? Title;
    }
}
