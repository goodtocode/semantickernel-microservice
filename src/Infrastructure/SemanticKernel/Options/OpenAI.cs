using System.ComponentModel.DataAnnotations;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Options;

/// <summary>
/// OpenAI settings.
/// </summary>
public sealed class OpenAI
{
    [Required]
    public string ChatCompletionModelId { get; set; } = string.Empty;

    [Required]
    public string TextGenerationModelId { get; set; } = string.Empty;

    [Required]
    public string TextEmbeddingModelId { get; set; } = string.Empty;

    [Required]
    public string TextModerationModelId { get; set; } = string.Empty;
    
    [Required]
    public string ImageModelId { get; set; } = string.Empty;

    [Required]
    public string AudioModelId { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
