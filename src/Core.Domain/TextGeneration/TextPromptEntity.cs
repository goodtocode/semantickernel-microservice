using Goodtocode.Domain.Types.DomainEntity;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

public class TextPromptEntity : DomainEntity<TextPromptEntity>
{
    protected TextPromptEntity() { }

    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Prompt { get; set; } = string.Empty;
    public virtual ICollection<TextResponseEntity> TextResponses { get; set; } = [];
    public virtual AuthorEntity? Author { get; set; }

    public static TextPromptEntity Create(Guid id, Guid authorId, string prompt)
    {
        return new TextPromptEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            AuthorId = authorId,
            Prompt = prompt,
            Timestamp = DateTime.UtcNow
        };
    }
}
