using Goodtocode.SemanticKernel.Presentation.Blazor.Models;
using Goodtocode.SemanticKernel.Presentation.Blazor.Utilities;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;

public interface IChatService
{
    Task SendMessageAsync(ChatMessageModel message);
    Task<List<ChatSessionModel>> GetChatSessionsAsync();
    Task<List<ChatMessageModel>>GetChatMessagesAsync(Guid chatSessionId);
}

public class ChatService(WebApiClient client, UserUtility userUtilityService) : IChatService
{
    private readonly WebApiClient _client = client;
    private readonly UserUtility _userUtilityService = userUtilityService;

    public async Task SendMessageAsync(ChatMessageModel message)
    {
        await _client.CreateChatSessionCommandAsync(new CreateChatSessionCommand { Message = message.Content }).ConfigureAwait(false);
    }

    public async Task<List<ChatSessionModel>> GetChatSessionsAsync()
    {
        var userId = await _userUtilityService.GetUserIdAsync();
        var response = await _client.GetAuthorChatSessionsPaginatedQueryAsync(userId, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, 1, 20).ConfigureAwait(false);

        return [.. response.Items.Select(dto => new ChatSessionModel
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
        })];
    }

    public Task<List<ChatMessageModel>> GetChatMessagesAsync(Guid chatSessionId)
    {
        throw new NotImplementedException();
    }
}

