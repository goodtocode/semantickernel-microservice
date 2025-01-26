using Goodtocode.SemanticKernel.Core.Domain.Image;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class TextImagesConfig : IEntityTypeConfiguration<TextImageEntity>
{
    public void Configure(EntityTypeBuilder<TextImageEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("TextImages");
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
            .WithMany(a => a.TextImages)
            .HasForeignKey(a => a.AuthorId);
        builder.Property(x => x.ImageBytes)
            .HasColumnType(ColumnTypes.VarbinaryMax);
    }
}