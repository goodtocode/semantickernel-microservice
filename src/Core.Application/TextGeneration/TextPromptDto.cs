using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class TextPromptDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid ActorId { get; set; } = Guid.Empty;
    public string Prompt { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }

    public static TextPromptDto CreateFrom(TextPromptEntity? entity)
    {
        if (entity is null) return null!;
        return new TextPromptDto
        {
            Id = entity.Id,
            ActorId = entity.ActorId,
            Prompt = entity.Prompt,
            Timestamp = entity.Timestamp,
        };
    }
}
