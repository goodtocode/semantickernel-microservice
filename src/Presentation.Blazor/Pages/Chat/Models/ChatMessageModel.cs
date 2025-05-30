namespace Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Models;

public class ChatMessageModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid ChatSessionId { get; set; } = Guid.Empty;
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
}