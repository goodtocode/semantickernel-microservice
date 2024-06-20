using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Domain.Subject;

public class AuthorChatSessionEntity : AuthorEntity
{
    public virtual ICollection<ChatSessionEntity> ChatSessions { get; set; } = [];
} 
