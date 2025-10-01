using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class TextPromptDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Prompt { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public ICollection<TextResponseDto>? Responses { get; set; }

    public static TextPromptDto CreateFrom(TextPromptEntity? entity)
    {
        if (entity is null) return null!;
        return new TextPromptDto
        {
            Id = entity.Id,
            AuthorId = entity.AuthorId,
            Prompt = entity.Prompt,
            Timestamp = entity.Timestamp,
            Responses = entity.TextResponses?
                .Select(TextResponseDto.CreateFrom)
                .ToList()
        };
    }
}
