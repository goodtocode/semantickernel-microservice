using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class AuthorsConfig : IEntityTypeConfiguration<AuthorChatSessionEntity>
{
    public void Configure(EntityTypeBuilder<AuthorChatSessionEntity> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(x => x.Key);
        builder.Property(x => x.Key);
        builder.Property(x => x.Name);
    }
}