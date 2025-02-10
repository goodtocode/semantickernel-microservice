using Microsoft.JSInterop;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Utilities;

public class UserUtility(IJSRuntime jsRuntime)
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;

    public const string UserIdKey = $"userId";

    public async Task<Guid> GetUserIdAsync()
    {
        var storedUserId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserIdKey);
        if (!Guid.TryParse(storedUserId, out Guid userId))
        {
            userId = Guid.NewGuid();
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserIdKey, storedUserId);
        }

        return userId!;
    }
}