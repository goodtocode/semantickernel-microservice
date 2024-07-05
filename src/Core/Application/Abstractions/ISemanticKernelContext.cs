using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;
using Goodtocode.SemanticKernel.Core.Domain.Image;

namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface ISemanticKernelContext
{
    DbSet<AuthorEntity> Authors { get; }
    DbSet<ChatMessageEntity> ChatMessages { get; }
    DbSet<ChatSessionEntity> ChatSessions { get; }
    DbSet<TextPromptEntity> TextPrompts { get; }
    DbSet<TextResponseEntity> TextResponses { get; }
    DbSet<TextImageEntity> TextImages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}