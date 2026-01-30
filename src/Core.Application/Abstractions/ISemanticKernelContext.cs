using Goodtocode.SemanticKernel.Core.Domain.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Audio;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface ISemanticKernelContext
{
    DbSet<ChatMessageEntity> ChatMessages { get; }
    DbSet<ChatSessionEntity> ChatSessions {get; }
    DbSet<TextPromptEntity> TextPrompts { get; }
    DbSet<TextResponseEntity> TextResponses { get; }
    DbSet<TextImageEntity> TextImages { get; }
    DbSet<TextAudioEntity> TextAudio { get; }
    DbSet<ActorEntity> Actors { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
#pragma warning disable CA1716 // Identifiers should not match keywords
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
#pragma warning restore CA1716
    IModel Model { get; }
}