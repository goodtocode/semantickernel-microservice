using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.Image;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class TextImageDto
{
    private int _width = 1024;
    private int _height = 1024;
    public Guid Id { get; set; } = Guid.Empty;
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
    public DateTimeOffset Timestamp { get; set; }

    public static TextImageDto CreateFrom(TextImageEntity? entity)
    {

        if (entity == null) return null!;
        return new TextImageDto
        {
            Id = entity.Id,
            AuthorId = entity.AuthorId,
            Description = entity.Description,
            ImageBytes = entity.ImageBytes,
            ImageUrl = entity.ImageUrl,
            Width = entity.Width,
            Height = entity.Height,
            Timestamp = entity.Timestamp
        };
    }
}
