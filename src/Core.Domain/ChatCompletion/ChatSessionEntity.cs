using Goodtocode.Domain.Types.DomainEntity;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using MediatR;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatSessionEntity : DomainEntity<ChatSessionEntity>
{
    protected ChatSessionEntity() { }

    public Guid AuthorId { get; set; }
    public string? Title { get; set; } = string.Empty;
    public virtual ICollection<ChatMessageEntity> Messages { get; set; } = [];
    public virtual AuthorEntity? Author { get; set; }

    public static ChatSessionEntity Create(Guid id, Guid authorId, string? title, ChatMessageRole responseRole, string initialMessage, string responseMessage)
    {
        var session = new ChatSessionEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            AuthorId = authorId,
            Title = title,
            Timestamp = DateTime.UtcNow
        };
        session.Messages.Add(ChatMessageEntity.Create(Guid.NewGuid(), session.Id, ChatMessageRole.user, initialMessage));
        session.Messages.Add(ChatMessageEntity.Create(Guid.NewGuid(), session.Id, responseRole, responseMessage));
        return session;
    }
}
