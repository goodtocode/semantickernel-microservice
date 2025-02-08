using Goodtocode.Domain.Types;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.Image;

public class TextImageEntity : DomainEntity<TextImageEntity>
{
    private int _width = 1024;
    private int _height = 1024;

    public TextImageEntity() { }

    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Description { get; set; } = string.Empty;
    public ReadOnlyMemory<byte>? ImageBytes { get; set; }
    public Uri? ImageUrl { get; set; }
    public int Height
    {
        get => _height;
        set => (_height, _width) = value switch
        {
            1024 => (1024, 1024),
            _ => throw new ArgumentOutOfRangeException("Height", "Must be 1024.")
        };
    }
    public int Width
    {
        get => _width;
        set => (_height, _width) = value switch
        {
            1024 => (1024, 1024),
            _ => throw new ArgumentOutOfRangeException("Width", "Must be 1024.")
        };
    }

    public virtual AuthorEntity Author { get; set; } = new();
}
