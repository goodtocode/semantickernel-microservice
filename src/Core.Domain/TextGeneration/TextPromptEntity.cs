using Goodtocode.Domain.Types;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

public class TextPromptEntity : DomainEntity<TextPromptEntity>
{
    public TextPromptEntity() { }

    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Prompt { get; set; } = string.Empty;
    public virtual ICollection<TextResponseEntity> TextResponses { get; set; } = [];

    public virtual AuthorEntity Author { get; set; } = new();
}
