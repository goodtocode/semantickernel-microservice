﻿using Goodtocode.Domain.Types;

namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

public class ChatSessionEntity : DomainEntity<ChatSessionEntity>
{
    public ChatSessionEntity() { }

    public string Title { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public virtual ICollection<ChatMessageEntity> Messages { get; set; } = [];
}
