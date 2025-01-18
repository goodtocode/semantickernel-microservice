using Goodtocode.SemanticKernel.Presentation.Blazor.Client.Models;
using System.Net.Http.Json;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Client.Services;
public interface IChatService
{
    Task SendMessageAsync(string message);
    Task<List<ChatSessionModel>> GetChatSessionsAsync();
}

public class ChatService(HttpClient httpClient) : IChatService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task SendMessageAsync(string message)
    {
        var response = await _httpClient.PostAsJsonAsync("api/chat/send", new { Message = message });
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ChatSessionModel>> GetChatSessionsAsync()
    {
        var response = new List<ChatSessionModel>(); // await _httpClient.GetFromJsonAsync<List<ChatSessionModel>>("api/chat/sessions");
        return response ?? [];
    }
}

