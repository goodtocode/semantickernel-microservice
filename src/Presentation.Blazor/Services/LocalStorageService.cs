using Microsoft.JSInterop;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;
public interface ILocalStorageService
{
    Task SetItemAsync(string key, string value);
    Task<string> GetItemAsync(string key);
}

public class LocalStorageService(IJSRuntime jsRuntime) : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;

    public async Task SetItemAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task<string> GetItemAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }
}
