using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class TextResponsesConfig : IEntityTypeConfiguration<TextResponseEntity>
{
    public void Configure(EntityTypeBuilder<TextResponseEntity> builder)
    {
        builder.ToTable("TextResponses");
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
    }
}