using System.Security.Claims;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Components.Auth.Services;

public interface IUserSyncService
{
    void UserChanged(ClaimsPrincipal? user);
    Task SyncUserAsync(ClaimsPrincipal? user);
}
