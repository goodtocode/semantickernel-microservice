namespace Goodtocode.SemanticKernel.Core.Domain.Auth;

public interface IUserEntity
{
    Guid OwnerId { get; }
    Guid TenantId { get; }
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    IEnumerable<string> Roles { get; }    
    bool CanView { get; }
    bool CanEdit { get; }
    bool CanDelete { get; }
}
