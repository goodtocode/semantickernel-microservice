using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class ChatMessageDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid ChatSessionId { get; set; } = Guid.Empty;
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }

    public static ChatMessageDto CreateFrom(ChatMessageEntity? entity)
    {
        if (entity == null) return null!;
        return new ChatMessageDto
        {
            Id = entity.Id,
            ChatSessionId = entity.ChatSessionId,
            Role = entity.Role.ToString(),
            Content = entity.Content,
            Timestamp = entity.Timestamp
        };
    }
}