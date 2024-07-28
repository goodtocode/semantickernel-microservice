using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class TextResponseDto : IMapFrom<TextResponseEntity>
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid TextPromptId { get; set; } = Guid.Empty;
    public string Response { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TextResponseEntity, TextResponseDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.TextPromptId, opt => opt.MapFrom(s => s.TextPromptId))
            .ForMember(d => d.Response, opt => opt.MapFrom(s => s.Response))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp));
    }
}