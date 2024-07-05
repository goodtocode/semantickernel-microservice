using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.Image;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class TextImageDto : IMapFrom<TextImageEntity>
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageBytes { get; set; }
    public Uri? ImageUrl { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TextImageEntity, TextImageDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d=> d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.ImageBytes, opt => opt.MapFrom(s => s.ImageBytes))
            .ForMember(d => d.ImageUrl, opt => opt.MapFrom(s => s.ImageUrl))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp));
    }
}
