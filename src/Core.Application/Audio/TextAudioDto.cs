using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.Audio;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class TextAudioDto : IMapFrom<TextAudioEntity>
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Description { get; set; } = string.Empty;
    public ReadOnlyMemory<byte>? AudioBytes { get; set; }
    public Uri? AudioUrl { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TextAudioEntity, TextAudioDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(d => d.AudioBytes, opt => opt.MapFrom(s => s.AudioBytes))
            .ForMember(d => d.AudioUrl, opt => opt.MapFrom(s => s.AudioUrl))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp));
    }
}
