using Goodtocode.SemanticKernel.Presentation.WebApi.Client;
using Goodtocode.SemanticKernel.Presentation.Blazor.Services;
using Cannery.Aspects.Components.Auth;
using Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Models;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Services;

public interface IChatService
{
    Task<List<ChatSessionModel>> GetChatSessionsAsync();
    Task<ChatSessionModel> GetChatSessionAsync(Guid chatSessionId);
    Task<ChatSessionModel> CreateSessionAsync(string firstMessage);
    Task RenameSessionAsync(Guid chatSessionId, string newTitle);
    Task<ChatMessageModel> SendMessageAsync(Guid chatSessionId, string newMessage);
}

public class ChatService(BackendApiClient client, IUserClaimsInfo userInfo) : ApiService, IChatService
{
    private readonly BackendApiClient _apiClient = client;
    private readonly IUserClaimsInfo _userInfo = userInfo;

    public async Task<List<ChatSessionModel>> GetChatSessionsAsync()
    {
        var response = await HandleApiException(() => _apiClient.GetMyChatSessionsPaginatedAsync(
            DateTime.UtcNow.AddDays(-30),
            DateTime.UtcNow,
            1,
            20
        ));

        return ChatSessionModel.Create(response.Items);
    }

    public async Task<ChatSessionModel> GetChatSessionAsync(Guid chatSessionId)
    {
        var response = await HandleApiException(() => _apiClient.GetMyChatSessionAsync(
            chatSessionId));

        return ChatSessionModel.Create(response);
    }

    public async Task<ChatSessionModel> CreateSessionAsync(string firstMessage)
    {
        var command = new CreateChatSessionCommand
        {
            ActorId = _userInfo.ObjectId,
            Message = firstMessage
        };
        var response = await HandleApiException(() => _apiClient.CreateChatSessionCommandAsync(command));

        return ChatSessionModel.Create(response);
    }

    public async Task RenameSessionAsync(Guid chatSessionId, string newTitle)
    {
        await HandleApiException(() => _apiClient.PatchChatSessionCommandAsync(chatSessionId, new PatchChatSessionCommand { Id = chatSessionId, Title = newTitle }));
    }

    public async Task<ChatMessageModel> SendMessageAsync(Guid chatSessionId, string newMessage)
    {
        var response = await HandleApiException(() => _apiClient.CreateChatMessageCommandAsync(
            new CreateChatMessageCommand
            {
                ChatSessionId = chatSessionId,
                Message = newMessage
            }));

        return ChatMessageModel.Create(response);
    }
}