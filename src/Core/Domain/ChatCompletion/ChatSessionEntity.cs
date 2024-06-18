using Goodtocode.Domain.Types;
using Goodtocode.SemanticKernel.Core.Domain.Subject;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatSessionEntity : DomainEntity<ChatSessionEntity>
{
    public ChatSessionEntity() { }

    public Guid AuthorKey { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public virtual ICollection<ChatMessageEntity> Messages { get; set; } = [];

    public virtual AuthorEntity Author { get; set; } = new();
}
