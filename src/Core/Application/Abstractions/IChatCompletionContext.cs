﻿using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IChatCompletionContext
{
    DbSet<AuthorChatSessionEntity> AuthorChatSessions { get; }
    DbSet<ChatMessageEntity> ChatMessages { get; }
    DbSet<ChatSessionEntity> ChatSessions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}