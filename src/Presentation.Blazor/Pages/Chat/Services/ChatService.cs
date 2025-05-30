using Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Models;
using Goodtocode.SemanticKernel.Presentation.Blazor.Services;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Services;

public interface IChatService
{    
    Task<List<ChatSessionModel>> GetChatSessionsAsync();
    Task<ChatSessionModel> GetChatSessionAsync(Guid chatSessionId);
    Task CreateSessionAsync(ChatSessionModel newSession, string firstMessage);
    Task SendMessageAsync(ChatSessionModel session, string newMessage);
}

public class ChatService(WebApiClient client, IUserService userUtilityService) : IChatService
{
    private readonly WebApiClient _client = client;
    private readonly IUserService _userService = userUtilityService;

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
            IsActive = false,
            Messages = [.. dto.Messages.Select(m => new ChatMessageModel
            {
                Id = m.Id,
                Content = m.Content,
                Role = m.Role,
                Timestamp = m.Timestamp
            })]
        })];
    }

    public async Task<ChatSessionModel> GetChatSessionAsync(Guid chatSessionId)
    {
        var userId = await _userService.GetUserIdAsync();
        var response = await _client.GetAuthorChatSessionQueryAsync(userId, chatSessionId).ConfigureAwait(false);

        return new ChatSessionModel
        {
            Id = response.Id,
            Title = response.Title,
            AuthorId = response.AuthorId,
            Timestamp = response.Timestamp,
            IsActive = false,
            Messages = [.. response.Messages.Select(m => new ChatMessageModel
            {
                Id = m.Id,
                Content = m.Content,
                Role = m.Role,
                Timestamp = m.Timestamp
            })]
        };
    }

    public async Task CreateSessionAsync(ChatSessionModel newSession, string firstMessage)
    {
        var command = new CreateChatSessionCommand
        {
            Id = newSession.Id,      
            AuthorId = newSession.AuthorId,
            Title = newSession.Title,
            Message = firstMessage
        };
        await _client.CreateChatSessionCommandAsync(command).ConfigureAwait(false);
    }

    public async Task SendMessageAsync(ChatSessionModel session, string newMessage)
    {        
        await _client.CreateChatMessageCommandAsync(new CreateChatMessageCommand { ChatSessionId = session.Id, Message = newMessage }).ConfigureAwait(false);
    }
}

