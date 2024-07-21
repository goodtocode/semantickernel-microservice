using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class ChatMessageDto : IMapFrom<ChatMessageEntity>
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid ChatSessionId { get; set; } = Guid.Empty;
    public ChatMessageRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ChatMessageEntity, ChatMessageDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.ChatSessionId, opt => opt.MapFrom(s => s.ChatSessionId))
            .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role))
            .ForMember(d => d.Content, opt => opt.MapFrom(s => s.Content))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp));
    }
}