using System.ComponentModel.DataAnnotations;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Options;

/// <summary>
/// OpenAI settings.
/// </summary>
public sealed class OpenAI
{
    [Required]
    public string ChatModelId { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
