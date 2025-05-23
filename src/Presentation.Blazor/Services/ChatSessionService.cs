using Goodtocode.SemanticKernel.Presentation.Blazor.Models;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;

public interface IChatService
{    
    Task<List<ChatSessionModel>> GetChatSessionsAsync();
    Task<List<ChatMessageModel>> GetChatSessionAsync(Guid chatSessionId);
    Task SendMessageAsync(ChatSessionModel session, ChatMessageModel message);
}

public class ChatService(WebApiClient client, IUserService userUtilityService) : IChatService
{
    private readonly WebApiClient _client = client;
    private readonly IUserService _userService = userUtilityService;


    public async Task SendMessageAsync(ChatSessionModel session, ChatMessageModel newMessage)
    {
        if (session == null)
            await _client.CreateChatSessionCommandAsync(new CreateChatSessionCommand { Message = newMessage.Content }).ConfigureAwait(false);
        else
            await _client.CreateChatMessageCommandAsync(new CreateChatMessageCommand{ChatSessionId = session.Id, Message = newMessage.Content}).ConfigureAwait(false);
    }

    public async Task<List<ChatSessionModel>> GetChatSessionsAsync()
    {
        var userId = await _userService.GetUserIdAsync();
        var response = await _client.GetAuthorChatSessionsPaginatedQueryAsync(userId, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, 1, 20).ConfigureAwait(false);

        return [.. response.Items.Select(dto => new ChatSessionModel
        {
            Id = dto.Id,
            Title = dto.Title,
            AuthorId = dto.AuthorId,
            Timestamp = dto.Timestamp,
            IsActive = false
        })];
    }

    public async Task<List<ChatMessageModel>> GetChatSessionAsync(Guid chatSessionId)
    {
        var userId = await _userService.GetUserIdAsync();
        var response = await _client.GetChatSessionQueryAsync(chatSessionId).ConfigureAwait(false);

        return [.. response.Messages.Select(dto =>  new ChatMessageModel
            {
                Id = dto.Id,
                Content = dto.Content,
                Timestamp = dto.Timestamp
            })];
    }
}

