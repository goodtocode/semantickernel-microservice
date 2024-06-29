using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class ChatSessionDto : IMapFrom<ChatSessionEntity>
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public DateTime Timestamp { get; set; }
    public virtual ICollection<ChatMessageDto>? Messages { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ChatSessionEntity, ChatSessionDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d=> d.Title, opt => opt.MapFrom(s => s.Title))
            .ForMember(d => d.Messages, opt => opt.MapFrom(s => s.Messages))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp));
    }
}
