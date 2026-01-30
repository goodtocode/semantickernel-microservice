using Goodtocode.Domain.Entities;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Domain.Image;

public class TextImageEntity : DomainEntity<TextImageEntity>
{
    private int _width = 1024;
    private int _height = 1024;

    protected TextImageEntity() { }

    public Guid ActorId { get; private set; } = Guid.Empty;
    public string Description { get; private set; } = string.Empty;
    public ReadOnlyMemory<byte>? ImageBytes { get; private set; }
    public Uri? ImageUrl { get; private set; }
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
    public virtual ActorEntity? Actor { get; set; }

    public static TextImageEntity Create(
        Guid id,
        string description,
        int width,
        int height,
        ReadOnlyMemory<byte>? imageBytes)
    {
        return Create(id, description, width, height, imageBytes, null);
    }

    public static TextImageEntity Create(
        Guid id,
        string description,
        int width,
        int height,
        Uri? imageUrl)
    {
        return Create(id, description, width, height, null, imageUrl);
    }

    public static TextImageEntity Create(
        Guid id,
        string description,
        int width,
        int height,
        ReadOnlyMemory<byte>? imageBytes,
        Uri? imageUrl)
    {
        return new TextImageEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Description = description,
            Width = width,
            Height = height,
            ImageBytes = imageBytes,
            ImageUrl = imageUrl,
            Timestamp = DateTime.UtcNow
        };
    }
}
