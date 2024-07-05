using Goodtocode.Domain.Types;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.Image;

public class TextImageEntity : DomainEntity<TextImageEntity>
{
    public TextImageEntity() { }

    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageBytes { get; set; }
    public Uri? ImageUrl { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public virtual AuthorEntity Author { get; set; } = new();
}
