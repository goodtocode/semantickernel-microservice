using Goodtocode.Domain.Types;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.Audio;

public class TextAudioEntity : DomainEntity<TextAudioEntity>
{
    public TextAudioEntity() { }

    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Description { get; set; } = string.Empty;
    public byte[]? AudioBytes { get; set; }
    public Uri? AudioUrl { get; set; }

    public virtual AuthorEntity Author { get; set; } = new();
}
