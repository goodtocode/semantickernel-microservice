using Goodtocode.Domain.Entities;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Audio;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;
using System.Reflection;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence;

public class SemanticKernelContext : DbContext, ISemanticKernelContext
{
    public DbSet<ChatMessageEntity> ChatMessages => Set<ChatMessageEntity>();
    public DbSet<ChatSessionEntity> ChatSessions => Set<ChatSessionEntity>();
    public DbSet<TextPromptEntity> TextPrompts => Set<TextPromptEntity>();
    public DbSet<TextResponseEntity> TextResponses => Set<TextResponseEntity>();
    public DbSet<TextImageEntity> TextImages => Set<TextImageEntity>();
    public DbSet<TextAudioEntity> TextAudio => Set<TextAudioEntity>();
    public DbSet<ActorEntity> Actors => Set<ActorEntity>();

    protected SemanticKernelContext() { }

    public SemanticKernelContext(DbContextOptions<SemanticKernelContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
            x => x.Namespace == $"{GetType().Namespace}.Configurations");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => IsDomainEntity(e.Entity) &&
                       (e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted));

        foreach (var entry in entries)
        {
            dynamic entity = entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.SetCreatedOn(DateTime.UtcNow);
                entity.SetModifiedOn(null);
                entity.SetDeletedOn(null);
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.SetModifiedOn(DateTime.UtcNow);
                entity.SetDeletedOn(null);
            }
            else if (entry.State == EntityState.Deleted)
            {
                entity.SetDeletedOn(DateTime.UtcNow);
                entry.State = EntityState.Modified;
            }
        }
    }

    private static bool IsDomainEntity(object entity)
    {
        var type = entity.GetType();
        while (type != null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DomainEntity<>))
                return true;
            type = type.BaseType;
        }
        return false;
    }
}
