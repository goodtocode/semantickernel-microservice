using System.Security.Claims;

namespace Goodtocode.Aspects.Components.Auth.Services;

public interface IUserSyncService
{
    void UserChanged(ClaimsPrincipal? user);
    Task SyncUserAsync(ClaimsPrincipal? user);
}
