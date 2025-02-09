namespace Goodtocode.SemanticKernel.Presentation.Blazor.Models;
public class ChatSessionModel
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public virtual ICollection<ChatMessageModel>? Messages { get; set; }

    public bool IsActive { get; set; }
}
