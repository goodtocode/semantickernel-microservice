using System.Security.Cryptography;
using System.Text;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services.Utilities;

public class UserUtility(IJSRuntime jsRuntime)
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;

    public const string UserIdKey = "userId";

    public async Task<string> GetUniqueUserIdAsync()
    {
        var userId = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserIdKey);
        if (string.IsNullOrEmpty(userId))
        {
            var computerName = Environment.MachineName;
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(computerName));
            userId = Convert.ToBase64String(hash);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserIdKey, userId);
        }
        return userId!;
    }
}