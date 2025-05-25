using Goodtocode.Domain.Types.DomainEntity;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using MediatR;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatSessionEntity : DomainEntity<ChatSessionEntity>
{
    private ChatSessionEntity() { }

    public Guid AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public virtual ICollection<ChatMessageEntity> Messages { get; set; } = [];
    public virtual AuthorEntity? Author { get; set; }

    public static ChatSessionEntity Create(Guid id, Guid authorId, string title, string initialMessage, string assistantMessage)
    {
        var session = new ChatSessionEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            AuthorId = authorId,
            Title = title
        };
        session.Messages.Add(ChatMessageEntity.Create(Guid.NewGuid(), session.Id, ChatMessageRole.user, initialMessage));
        session.Messages.Add(ChatMessageEntity.Create(Guid.NewGuid(), session.Id, ChatMessageRole.assistant, assistantMessage));
        return session;
    }

    public static ChatSessionEntity Create(Guid id, Guid authorId, string title, string initialMessage, string assistantMessage, DateTime timestamp)
    {
        var session = ChatSessionEntity.Create(id, authorId, title, initialMessage, assistantMessage);
        session.Timestamp = timestamp;
        return session;
    }
}
