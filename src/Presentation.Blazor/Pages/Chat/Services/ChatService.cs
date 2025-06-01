using Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Models;
using Goodtocode.SemanticKernel.Presentation.Blazor.Services;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Services;

public interface IChatService
{    
    Task<List<ChatSessionModel>> GetChatSessionsAsync();
    Task<ChatSessionModel> GetChatSessionAsync(Guid chatSessionId);
    Task<ChatSessionModel> CreateSessionAsync(string firstMessage);
    Task<ChatMessageModel> SendMessageAsync(Guid chatSessionId, string newMessage);
}

public class ChatService(WebApiClient client, IUserService userUtilityService) : IChatService
{
    private readonly WebApiClient _client = client;
    private readonly IUserService _userService = userUtilityService;

    public async Task<List<ChatSessionModel>> GetChatSessionsAsync()
    {
        var userId = await _userService.GetUserIdAsync();
        var response = await _client.GetAuthorChatSessionsPaginatedQueryAsync(userId, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, 1, 20).ConfigureAwait(false);

        return ChatSessionModel.Create(response.Items);
    }

    public async Task<ChatSessionModel> GetChatSessionAsync(Guid chatSessionId)
    {
        var userId = await _userService.GetUserIdAsync();
        var response = await _client.GetAuthorChatSessionQueryAsync(userId, chatSessionId).ConfigureAwait(false);

        return ChatSessionModel.Create(response);
    }

    public async Task<ChatSessionModel> CreateSessionAsync(string firstMessage)
    {
        var command = new CreateChatSessionCommand
        {
            AuthorId = await _userService.GetUserIdAsync(),
            Message = firstMessage
        };
        var response = await _client.CreateChatSessionCommandAsync(command).ConfigureAwait(false);        

        return ChatSessionModel.Create(response);
    }

    public async Task RenameSessionAsync(Guid chatSessionId, string newTitle)
    {
        await _client.PatchChatSessionCommandAsync(chatSessionId, new PatchChatSessionCommand { Id = chatSessionId,  Title = newTitle }).ConfigureAwait(false);
    }

    public async Task<ChatMessageModel> SendMessageAsync(Guid chatSessionId, string newMessage)
    {        
        var response = await _client.CreateChatMessageCommandAsync(new CreateChatMessageCommand { ChatSessionId = chatSessionId, Message = newMessage }).ConfigureAwait(false);

        return ChatMessageModel.Create(response);
    }
}

