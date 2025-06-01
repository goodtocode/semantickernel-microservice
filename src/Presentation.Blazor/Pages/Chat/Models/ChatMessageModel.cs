using Goodtocode.SemanticKernel.Presentation.WebApi.Client;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Models;

public class ChatMessageModel
{

    public static ChatMessageModel Create(ChatMessageDto chatMessage)
    {
        return new ChatMessageModel
        {
            Id = chatMessage.Id,
            ChatSessionId = chatMessage.ChatSessionId,
            Role = chatMessage.Role,
            Content = chatMessage.Content,
            Timestamp = chatMessage.Timestamp
        };
    }

    public Guid Id { get; set; } = Guid.Empty;
    public Guid ChatSessionId { get; set; } = Guid.Empty;
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
}