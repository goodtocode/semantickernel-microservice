using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Domain.Author;

public class AuthorChatSessionEntity : AuthorEntity
{
    public virtual ICollection<ChatSessionEntity> ChatSessions { get; set; } = [];
} 
