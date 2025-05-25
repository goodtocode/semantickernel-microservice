using Goodtocode.Domain.Types.DomainEntity;
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
    public static TextAudioEntity Create(Guid authorId, string description, ReadOnlyMemory<byte>? audioBytes = null, Uri? audioUrl = null)
    {
        return new TextAudioEntity
        {
            AuthorId = authorId == Guid.Empty ? Guid.NewGuid() : authorId,
            Description = description,
            AudioBytes = audioBytes,
            AudioUrl = audioUrl
        };
    }
}
