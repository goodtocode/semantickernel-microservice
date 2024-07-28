using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class TextPromptDto : IMapFrom<TextPromptEntity>
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public string Prompt { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public virtual ICollection<TextResponseDto>? Responses { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<TextPromptEntity, TextPromptDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d=> d.Prompt, opt => opt.MapFrom(s => s.Prompt))
            .ForMember(d => d.Responses, opt => opt.MapFrom(s => s.TextResponses))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp));
    }
}
