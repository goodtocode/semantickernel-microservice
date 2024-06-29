using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class ChatSessionsConfig : IEntityTypeConfiguration<ChatSessionEntity>
{
    public void Configure(EntityTypeBuilder<ChatSessionEntity> builder)
    {
        builder.ToTable("ChatSessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.Timestamp);
        builder
            .HasOne(a => a.Author)
            .WithMany(a => a.ChatSessions)
            .HasForeignKey(a => a.AuthorId);
        builder
            .HasMany(cs => cs.Messages)
            .WithOne(cm => cm.ChatSession)
            .HasForeignKey(cm => cm.ChatSessionId);
    }
}