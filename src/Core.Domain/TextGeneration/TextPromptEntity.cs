using Goodtocode.Domain.Entities;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

public class TextPromptEntity : DomainEntity<TextPromptEntity>
{
    protected TextPromptEntity() { }

    public Guid ActorId { get; private set; } = Guid.Empty;
    public string Prompt { get; private set; } = string.Empty;
    public virtual ActorEntity? Actor { get; private set; }

    public static TextPromptEntity Create(Guid id, Guid authorId, string prompt)
    {
        return new TextPromptEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            ActorId = authorId,
            Prompt = prompt,
            Timestamp = DateTime.UtcNow
        };
    }
}
