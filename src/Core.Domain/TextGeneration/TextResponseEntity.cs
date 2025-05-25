using Goodtocode.Domain.Types.DomainEntity;

namespace Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

public class TextResponseEntity : DomainEntity<TextResponseEntity>
{
    private TextResponseEntity() { }

    public Guid TextPromptId { get; set; } = Guid.Empty;
    public string Response { get; set; } = string.Empty;
    public virtual TextPromptEntity? TextPrompt { get; set; }

    public static TextResponseEntity Create(Guid id, Guid textPromptId, string response)
    {
        return new TextResponseEntity
        {
            Id = id,
            TextPromptId = textPromptId,
            Response = response
        };
    }
}
