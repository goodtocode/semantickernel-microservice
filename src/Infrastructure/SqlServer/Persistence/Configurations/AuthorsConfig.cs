using Goodtocode.SemanticKernel.Core.Domain.Subject;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class AuthorsConfig : IEntityTypeConfiguration<AuthorEntity>
{
    public void Configure(EntityTypeBuilder<AuthorEntity> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(x => x.Key);
        builder.Property(x => x.Key);
        builder.Property(x => x.Name);
    }
}