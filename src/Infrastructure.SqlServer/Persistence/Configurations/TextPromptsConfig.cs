using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class TextPromptsConfig : IEntityTypeConfiguration<TextPromptEntity>
{
    public void Configure(EntityTypeBuilder<TextPromptEntity> builder)
    {
        builder.ToTable("TextPrompts");
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
            .WithMany(a => a.TextPrompts)
            .HasForeignKey(a => a.AuthorId);
        builder
            .HasMany(cs => cs.TextResponses)
            .WithOne(cm => cm.TextPrompt)
            .HasForeignKey(cm => cm.TextPromptId);
    }
}