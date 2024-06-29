using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class AuthorsConfig : IEntityTypeConfiguration<AuthorEntity>
{
    public void Configure(EntityTypeBuilder<AuthorEntity> builder)
    {
        builder.ToTable("Authors");
        builder.HasIndex(x => x.Id)
            .IsClustered(false)
            .IsUnique();
        builder.HasIndex(x => x.Timestamp)
            .IsClustered()
            .IsUnique();
        builder.HasKey(x => x.Id)
            .IsClustered(false);
        builder.Property(x => x.Id)
            .HasDefaultValue(Guid.NewGuid());
        builder.Property(x => x.Name)
            .HasColumnType(ColumnTypes.Nvarchar200);
    }
}