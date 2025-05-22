using Goodtocode.SemanticKernel.Presentation.Blazor.Models;
using Goodtocode.SemanticKernel.Presentation.Blazor.Utilities;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;

public interface IChatService
{
    Task SendMessageAsync(ChatMessageModel message);
    Task<List<ChatSessionModel>> GetChatSessionsAsync();
    Task<List<ChatMessageModel>> GetChatSessionAsync(Guid chatSessionId);
}

public class ChatService(WebApiClient client, UserUtility userUtilityService) : IChatService
{
    private readonly WebApiClient _client = client;
    private readonly UserUtility _userService = userUtilityService;

    public async Task SendMessageAsync(ChatMessageModel message)
    {
        await _client.CreateChatSessionCommandAsync(new CreateChatSessionCommand { Message = message.Content }).ConfigureAwait(false);
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

