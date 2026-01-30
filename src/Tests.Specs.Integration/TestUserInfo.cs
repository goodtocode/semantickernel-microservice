using Goodtocode.SemanticKernel.Core.Domain.Auth;

namespace Goodtocode.SemanticKernel.Specs.Integration;

public class TestUserInfo() : IUserEntity
{
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _tenantId = Guid.NewGuid();
    public Guid OwnerId => _userId;
    public Guid TenantId => _tenantId;
    public string FirstName => "John";
    public string LastName => "Tester";
    public string Email => "John.Tester@goodtocode.com";
    public IEnumerable<string> Roles => ["Admin"];
    public bool CanView => true;
    public bool CanEdit => true;
    public bool CanDelete => true;
}
