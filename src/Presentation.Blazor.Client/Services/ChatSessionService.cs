using Goodtocode.Presentation.WebApi.Client;
using Goodtocode.SemanticKernel.Presentation.Blazor.Client.Models;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Client.Services;
public interface IChatService
{
    Task SendMessageAsync(string message);
    Task<List<ChatSessionModel>> GetChatSessionsAsync();
}

public class ChatService(WebApiClient client) : IChatService
{
    private readonly WebApiClient _client = client;

    public async Task SendMessageAsync(string message)
    {
        await _client.CreateChatSessionCommandAsync(new CreateChatSessionCommand { Message = message });
    }

    public async Task<List<ChatSessionModel>> GetChatSessionsAsync()
    {
        var response =  await _client.GetChatSessionsQueryAsync();
        
        return response.Select(dto => new ChatSessionModel
         {
             Id = dto.Id,
             Title = dto.Title,
             AuthorId = dto.AuthorId,
             Timestamp = dto.Timestamp,
             IsActive = false,
             Messages = dto.Messages?.Select(m => new ChatMessageModel
             {
                 Id = m.Id,
                 Content = m.Content,
                 Timestamp = m.Timestamp
             }).ToList()
         }).ToList();
    }
}

