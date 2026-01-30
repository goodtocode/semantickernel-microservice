using Goodtocode.SemanticKernel.Core.Domain.Audio;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class TextAudioConfig : IEntityTypeConfiguration<TextAudioEntity>
{
    public void Configure(EntityTypeBuilder<TextAudioEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("TextAudio");
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
        builder.Property(x => x.AudioBytes)
            .HasColumnType(ColumnTypes.VarbinaryMax)
            .HasConversion(
            v => v.HasValue ? v.Value.ToArray() : null,
            v => v != null ? new ReadOnlyMemory<byte>(v) : null);
    }
}