using Goodtocode.Domain.Types;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Domain.Author;

public class AuthorEntity : DomainEntity<AuthorEntity>
{
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<ChatSessionEntity> ChatSessions { get; set; } = [];
    public virtual ICollection<TextPromptEntity> TextPrompts { get; set; } = [];
}
