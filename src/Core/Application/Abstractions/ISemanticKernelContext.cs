using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface ISemanticKernelContext
{
    DbSet<AuthorEntity> Authors { get; }
    DbSet<ChatMessageEntity> ChatMessages { get; }
    DbSet<ChatSessionEntity> ChatSessions { get; }
    DbSet<TextPromptEntity> TextPrompts { get; }
    DbSet<TextResponseEntity> TextResponses { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}