using Goodtocode.Domain.Types;

namespace Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

public class TextResponseEntity : DomainEntity<TextResponseEntity>
{
    public TextResponseEntity() { }

    public Guid TextPromptId { get; set; } = Guid.Empty;
    public string Response { get; set; } = string.Empty;

    public virtual TextPromptEntity TextPrompt { get; set; } = new();
}
