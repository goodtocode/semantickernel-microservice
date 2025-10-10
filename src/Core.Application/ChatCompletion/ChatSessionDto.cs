using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class ChatSessionDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public Guid AuthorId { get; set; } = Guid.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public ICollection<ChatMessageDto>? Messages { get; set; }

    public static ChatSessionDto CreateFrom(ChatSessionEntity? entity)
    {
        if (entity is null) return null!;
        return new ChatSessionDto
        {
            Id = entity.Id,
            Title = entity.Title ?? string.Empty,
            AuthorId = entity.AuthorId,
            Timestamp = entity.Timestamp,
            Messages = entity.Messages?.Select(ChatMessageDto.CreateFrom).ToList()
        };
    }
}
