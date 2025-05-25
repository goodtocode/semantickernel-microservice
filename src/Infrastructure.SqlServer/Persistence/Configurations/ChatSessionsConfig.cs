﻿using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class ChatSessionsConfig : IEntityTypeConfiguration<ChatSessionEntity>
{
    public void Configure(EntityTypeBuilder<ChatSessionEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("ChatSessions");
        builder.HasKey(x => x.Id)
            .IsClustered(false);
        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .IsUnique();
        builder.HasIndex(x => x.Timestamp)
            .IsClustered()
            .IsUnique();
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        builder.Ignore(x => x.PartitionKey);
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