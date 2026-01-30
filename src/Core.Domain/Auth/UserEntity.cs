namespace Goodtocode.SemanticKernel.Core.Domain.Auth;

public struct UserRoles
{
    public const string ChatOwner = "AssetOwner";
    public const string ChatEditor = "AssetEditor";
    public const string ChatViewer = "AssetViewer";    
}

public class UserEntity() : IUserEntity
{
    public Guid OwnerId { get; private set; }
    public Guid TenantId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public IEnumerable<string> Roles { get; private set; } = [];
    public bool CanView => Roles.Contains(UserRoles.ChatOwner) || Roles.Contains(UserRoles.ChatEditor) || Roles.Contains(UserRoles.ChatViewer);
    public bool CanEdit => Roles.Contains(UserRoles.ChatOwner) || Roles.Contains(UserRoles.ChatEditor);
    public bool CanDelete => Roles.Contains(UserRoles.ChatOwner);

    public static UserEntity Create(Guid ownerId, Guid tenantId, string firstName, string lastName, string email, IEnumerable<string> roles)
    {
        return new UserEntity
        {
            OwnerId = ownerId,
            TenantId = tenantId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Roles = roles
        };
    }

}