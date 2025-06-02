using Goodtocode.Domain.Types.DomainEntity;
using Goodtocode.SemanticKernel.Core.Domain.Audio;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Domain.Author;

public class AuthorEntity : DomainEntity<AuthorEntity>
{
    protected AuthorEntity() { }
    
    public string? Name { get; set; } = string.Empty;

    public virtual ICollection<ChatSessionEntity> ChatSessions { get; private set; } = [];
    public virtual ICollection<TextPromptEntity> TextPrompts { get; private set; } = [];
    public virtual ICollection<TextImageEntity> TextImages { get; private set; } = [];
    public virtual ICollection<TextAudioEntity> TextAudio { get; private set; } = [];
    public static AuthorEntity Create(Guid id, string? name)
    {
        return new AuthorEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Name = name
        };
    }
}
