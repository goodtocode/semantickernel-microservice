using Goodtocode.Domain.Entities;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Domain.Audio;

public class TextAudioEntity : DomainEntity<TextAudioEntity>
{
    private ReadOnlyMemory<byte>? _audioBytes;

    protected TextAudioEntity() { }

    public Guid ActorId { get; private set; } = Guid.Empty;

    public string Description { get; private set; } = string.Empty;

    public ReadOnlyMemory<byte>? AudioBytes
    {
        get => _audioBytes;
        set => _audioBytes = value.HasValue ? value.Value.ToArray() : null;
    }

    public Uri? AudioUrl { get; private set; }

    public virtual ActorEntity Actor { get; private set; } = default!;
    public static TextAudioEntity Create(Guid id, Guid authorId, string description, ReadOnlyMemory<byte>? audioBytes)
    {
        return Create(id, authorId, description, audioBytes, null);
    }

    public static TextAudioEntity Create(Guid id, Guid authorId, string description, Uri? audioUrl)
    {
        return Create(id, authorId, description, null, audioUrl);
    }

    public static TextAudioEntity Create(Guid id, Guid authorId, string description, ReadOnlyMemory<byte>? audioBytes, Uri? audioUrl)
    {
        return new TextAudioEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            ActorId = authorId,
            Description = description,
            AudioBytes = audioBytes,
            AudioUrl = audioUrl,
            Timestamp = DateTime.UtcNow
        };
    }
}
