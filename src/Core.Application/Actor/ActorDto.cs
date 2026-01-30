using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class ActorDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public Guid TenantId { get; set; }
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedOn { get; private set; }
    public DateTime? DeletedOn { get; private set; }

    public static ActorDto CreateFrom(ActorEntity? entity)
    {
        if (entity is null) return null!;
        return new ActorDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName ?? string.Empty,
            LastName = entity.LastName ?? string.Empty,
            Email = entity.Email ?? string.Empty,
            OwnerId = entity.OwnerId,
            TenantId = entity.TenantId,
            CreatedOn = entity.CreatedOn,
            ModifiedOn = entity.ModifiedOn,
            DeletedOn = entity.DeletedOn
        };
    }
}
