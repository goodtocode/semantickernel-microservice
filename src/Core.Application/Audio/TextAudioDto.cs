using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.Audio;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class TextAudioDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Description { get; set; } = string.Empty;
    public ReadOnlyMemory<byte>? AudioBytes { get; set; }
    public Uri? AudioUrl { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public static TextAudioDto CreateFrom(TextAudioEntity? entity)
    {
        if (entity is null) return null!;
        return new TextAudioDto
        {
            Id = entity.Id,
            AuthorId = entity.AuthorId,
            Description = entity.Description,
            AudioBytes = entity.AudioBytes,
            AudioUrl = entity.AudioUrl,
            Timestamp = entity.Timestamp
        };
    }
}
