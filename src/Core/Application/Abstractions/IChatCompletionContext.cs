using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities;
using Goodtocode.SemanticKernel.Core.Domain.Subject;

namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface IChatCompletionContext
{
    DbSet<AuthorEntity> Authors { get; }
    DbSet<ChatMessageEntity> ChatMessages { get; }
    DbSet<ChatSessionEntity> ChatSessions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}