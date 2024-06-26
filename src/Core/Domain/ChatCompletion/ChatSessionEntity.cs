﻿using Goodtocode.Domain.Types;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatSessionEntity : DomainEntity<ChatSessionEntity>
{
    public ChatSessionEntity() { }

    public Guid AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public virtual ICollection<ChatMessageEntity> Messages { get; set; } = [];
    public virtual AuthorEntity Author { get; set; } = new();
}
