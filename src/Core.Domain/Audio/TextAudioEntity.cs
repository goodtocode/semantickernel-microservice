using Goodtocode.Domain.Types;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.Audio;

public class TextAudioEntity : DomainEntity<TextAudioEntity>
{
    private ReadOnlyMemory<byte>? _audioBytes;

    public TextAudioEntity() { }

    public Guid AuthorId { get; set; } = Guid.Empty;

    public string Description { get; set; } = string.Empty;

    public ReadOnlyMemory<byte>? AudioBytes
    {
        get => _audioBytes;
        set => _audioBytes = value.HasValue ? value.Value.ToArray() : null;
    }

    public Uri? AudioUrl { get; set; }

    public virtual AuthorEntity Author { get; set; } = default!;
}
