using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using System.Reflection;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence;

public class SemanticKernelContext : DbContext, ISemanticKernelContext
{
    // Roles: User, Assistant, System
    public DbSet<AuthorEntity> Authors => Set<AuthorEntity>();
    public DbSet<ChatMessageEntity> ChatMessages => Set<ChatMessageEntity>();
    public DbSet<ChatSessionEntity> ChatSessions => Set<ChatSessionEntity>();
    public DbSet<TextPromptEntity> TextPrompts => Set<TextPromptEntity>();
    public DbSet<TextResponseEntity> TextResponses => Set<TextResponseEntity>();

    protected SemanticKernelContext() { }

    public SemanticKernelContext(DbContextOptions<SemanticKernelContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
            x => x.Namespace == $"{GetType().Namespace}.Configurations");
    }
}
