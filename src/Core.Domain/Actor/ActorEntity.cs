using Goodtocode.SemanticKernel.Core.Domain.Auth;
using Goodtocode.Domain.Entities;

namespace Goodtocode.SemanticKernel.Core.Domain.Actor;

public class ActorEntity : SecuredEntity<ActorEntity>
{
    protected ActorEntity() { }

    public string? FirstName { get; private set; } = string.Empty;
    public string? LastName { get; private set; } = string.Empty;
    public string? Email { get; private set; } = string.Empty;

    public static ActorEntity Create(Guid id, Guid ownerId, Guid tenantId, string? firstName, string? lastName, string? email)
    {
        return new ActorEntity
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,            
            OwnerId = ownerId,
            TenantId = tenantId,
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };
    }

    public static ActorEntity Create(IUserEntity userInfo)
    {
        return new ActorEntity
        {
            Id = Guid.NewGuid(),
            OwnerId = userInfo.OwnerId,
            TenantId = userInfo.TenantId,
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            Email = userInfo.Email
        };
    }

    public void Update(string? firstName, string? lastName, string? email)
    {
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            Email = email ?? Email;
    }
}
