using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class ChatSessionsConfig : IEntityTypeConfiguration<ChatSessionEntity>
{
    public void Configure(EntityTypeBuilder<ChatSessionEntity> builder)
    {
        builder.ToTable("ChatSessions");
        builder.HasKey(x => x.Key);
        builder.Property(x => x.Key);
        builder.Property(x => x.Timestamp);
        builder            
            .HasMany(cs => cs.Messages) // ChatSession has many ChatMessages
            .WithOne(cm => cm.ChatSession) // ChatMessage has one ChatSession
            .HasForeignKey(cm => cm.ChatSessionKey); // Foreign key property
    }
}