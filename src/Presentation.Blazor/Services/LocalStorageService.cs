using Microsoft.JSInterop;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;

public static class LocalStorageFunctions
{
    public const string GetItem = "localStorage.getItem";
    public const string SetItem = "localStorage.setItem";
    public const string RemoveItem = "localStorage.removeItem";
    public const string Clear = "localStorage.clear";
}

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
        await _jsRuntime.InvokeVoidAsync(LocalStorageFunctions.SetItem, key, value);
    }

    public async Task<string> GetItemAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>(LocalStorageFunctions.GetItem, key);
    }
}
