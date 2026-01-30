using System.ComponentModel.DataAnnotations;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Options;

/// <summary>
/// Presentation.WebApi settings
/// </summary>
public sealed class BackendApiOptions
{
    public const string SectionName = "BackendApi";

    [Required]
    public Uri BaseUrl { get; set; } = default!;
}
