using Goodtocode.Domain.Types.DomainEntity;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.Image;

public class TextImageEntity : DomainEntity<TextImageEntity>
{
    private int _width = 1024;
    private int _height = 1024;

    protected TextImageEntity() { }

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
    public virtual AuthorEntity? Author { get; set; }

    public static TextImageEntity Create(
        Guid id,
        string description,
        int width,
        int height,
        ReadOnlyMemory<byte>? imageBytes,
        DateTime timestamp)
    {
        return TextImageEntity.Create(id, description, width, height, imageBytes, null, timestamp);
    }

    public static TextImageEntity Create(
        Guid id,
        string description,
        int width,
        int height,
        Uri? imageUrl,
        DateTime timestamp)
    {
        return TextImageEntity.Create(id, description, width, height, null, imageUrl, timestamp);
    }

    public static TextImageEntity Create(
        Guid id,
        string description,
        int width,
        int height,
        ReadOnlyMemory<byte>? imageBytes,
        Uri? imageUrl,
        DateTime timestamp)
    {
        return new TextImageEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Description = description,
            Width = width,
            Height = height,
            ImageBytes = imageBytes,
            ImageUrl = imageUrl,
            Timestamp = timestamp
        };
    }
}
