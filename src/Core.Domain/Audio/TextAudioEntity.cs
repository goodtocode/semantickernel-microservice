using Goodtocode.Domain.Types.DomainEntity;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.Audio;

public class TextAudioEntity : DomainEntity<TextAudioEntity>
{
    private ReadOnlyMemory<byte>? _audioBytes;

    protected TextAudioEntity() { }

    public Guid AuthorId { get; set; } = Guid.Empty;

    public string Description { get; set; } = string.Empty;

    public ReadOnlyMemory<byte>? AudioBytes
    {
        get => _audioBytes;
        set => _audioBytes = value.HasValue ? value.Value.ToArray() : null;
    }

    public Uri? AudioUrl { get; set; }

    public virtual AuthorEntity Author { get; set; } = default!;
    public static TextAudioEntity Create(Guid id, Guid authorId, string description, ReadOnlyMemory<byte>? audioBytes)
    {
        return TextAudioEntity.Create(id, authorId, description, audioBytes, null);
    }

    public static TextAudioEntity Create(Guid id, Guid authorId, string description, Uri? audioUrl)
    {
        return TextAudioEntity.Create(id, authorId, description, null, audioUrl);
    }

    public static TextAudioEntity Create(Guid id, Guid authorId, string description, ReadOnlyMemory<byte>? audioBytes, Uri? audioUrl)
    {
        return new TextAudioEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            AuthorId = authorId,
            Description = description,
            AudioBytes = audioBytes,
            AudioUrl = audioUrl,
            Timestamp = DateTime.UtcNow
        };
    }
}
