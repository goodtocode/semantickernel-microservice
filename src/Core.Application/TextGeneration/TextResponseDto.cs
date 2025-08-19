using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class TextResponseDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid TextPromptId { get; set; } = Guid.Empty;
    public string Response { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }

    public static TextResponseDto CreateFrom(TextResponseEntity entity)
    {
        if (entity is null) return null!;
        return new TextResponseDto
        {
            Id = entity.Id,
            TextPromptId = entity.TextPromptId,
            Response = entity.Response,
            Timestamp = entity.Timestamp
        };
    }
}