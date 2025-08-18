using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class AuthorDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedOn { get; private set; }
    public DateTime? DeletedOn { get; private set; }

    public static AuthorDto CreateFrom(AuthorEntity? entity)
    {
        if (entity == null) return null!;
        return new AuthorDto
        {
            Id = entity.Id,
            Name = entity.Name ?? string.Empty,
            CreatedOn = entity.CreatedOn,
            ModifiedOn = entity.ModifiedOn,
            DeletedOn = entity.DeletedOn
        };
    }
}
