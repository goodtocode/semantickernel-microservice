using System.ComponentModel.DataAnnotations;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Options;

/// <summary>
/// Presentation.WebApi settings
/// </summary>
public sealed class WebApiOptions
{
    public const string SectionName = "WebApi";

    [Required]
    public Uri BaseUrl { get; set; } = default!;
}
