using Goodtocode.SemanticKernel.Presentation.WebApi.Client;
using Goodtocode.SemanticKernel.Presentation.Blazor.Models;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;
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
        await _client.CreateChatSessionCommandAsync(new CreateChatSessionCommand { Message = message }).ConfigureAwait(false);
    }

    public async Task<List<ChatSessionModel>> GetChatSessionsAsync()
    {
        var response =  await _client.GetChatSessionsQueryAsync().ConfigureAwait(false);
        
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

