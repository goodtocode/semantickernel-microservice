using Goodtocode.Domain.Entities;

namespace Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

public class TextResponseEntity : DomainEntity<TextResponseEntity>
{
    protected TextResponseEntity() { }

    public Guid TextPromptId { get; private set; } = Guid.Empty;
    public string Response { get; private set; } = string.Empty;
    public virtual TextPromptEntity? TextPrompt { get; private set; }

    public static TextResponseEntity Create(Guid id, Guid textPromptId, string response)
    {
        return new TextResponseEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            TextPromptId = textPromptId,
            Response = response
        };
    }
}
