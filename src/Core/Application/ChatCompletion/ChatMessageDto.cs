using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class ChatMessageDto : IMapFrom<ChatMessageEntity>
{
    public Guid Key { get; set; } = Guid.Empty;
    public Guid ChatSessionKey { get; set; } = Guid.Empty;
    public ChatMessageRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ChatMessageEntity, ChatMessageDto>()
            .ForMember(d => d.Key, opt => opt.MapFrom(s => s.Key))
            .ForMember(d => d.ChatSessionKey, opt => opt.MapFrom(s => s.ChatSessionKey))
            .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role))
            .ForMember(d => d.Content, opt => opt.MapFrom(s => s.Content))
            .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => s.Timestamp));
    }
}