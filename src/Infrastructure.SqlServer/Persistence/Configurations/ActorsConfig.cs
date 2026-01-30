using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class ActorsConfig : IEntityTypeConfiguration<ActorEntity>
{
    public void Configure(EntityTypeBuilder<ActorEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Actors");

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

        builder.Property(x => x.FirstName)
            .HasColumnType(ColumnTypes.Nvarchar200);

        builder.Property(x => x.LastName)
            .HasColumnType(ColumnTypes.Nvarchar200);

        builder.Property(x => x.Email)
            .HasColumnType(ColumnTypes.Nvarchar200);

        builder.Property(x => x.OwnerId)
            .HasColumnType(ColumnTypes.Uniqueidentifier)
            .IsRequired();

        builder.Property(x => x.TenantId)
            .HasColumnType(ColumnTypes.Uniqueidentifier)
            .IsRequired();

        builder.HasIndex(x => new { x.TenantId, x.OwnerId })
            .IsUnique();
    }
}