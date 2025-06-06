using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.Image;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class TextImageDto : IMapFrom<TextImageEntity>
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

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TextImageEntity, TextImageDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.ImageBytes, opt => opt.MapFrom(s => s.ImageBytes))
            .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.ImageUrl))
            .ForMember(d => d.Width, opt => opt.MapFrom(s => s.Width))
            .ForMember(d => d.Height, opt => opt.MapFrom(s => s.Height))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp));
    }
}
