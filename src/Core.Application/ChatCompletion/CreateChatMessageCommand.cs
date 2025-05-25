using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatMessageCommand : IRequest<ChatMessageDto>
{
    public Guid Id { get; set; }
    public Guid ChatSessionId { get; set; }
    public string? Message { get; set; }
}

public class CreateChatMessageCommandHandler(IChatCompletionService chatService, ISemanticKernelContext context, IMapper mapper) : IRequestHandler<CreateChatMessageCommand, ChatMessageDto>
{
    private readonly IChatCompletionService _chatService = chatService;
    private readonly IMapper _mapper = mapper;
    private readonly ISemanticKernelContext _context = context;

    public async Task<ChatMessageDto> Handle(CreateChatMessageCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstSessionNotFound(_context.ChatSessions, request!.ChatSessionId);
        GuardAgainstEmptyMessage(request?.Message);
        GuardAgainstIdExsits(_context.ChatMessages, request!.Id);

        var chatSession = _context.ChatSessions.Find(request.ChatSessionId);
        var chatHistory = new ChatHistory();
        foreach (ChatMessageEntity message in chatSession!.Messages)
        {
            chatHistory.AddUserMessage(message.Content);
        }

        chatHistory.AddUserMessage(request!.Message!);
        var response = await _chatService.GetChatMessageContentAsync(chatHistory, null, null, cancellationToken);

        var chatMessage = ChatMessageEntity.Create(Guid.NewGuid(), chatSession.Id, ChatMessageRole.user, request.Message!, DateTime.UtcNow);
        chatSession.Messages.Add(chatMessage);
        _context.ChatMessages.Add(chatMessage);

        var chatMessageResponse = ChatMessageEntity.Create(Guid.NewGuid(), 
            chatSession.Id, 
            Enum.TryParse<ChatMessageRole>(response.Role.ToString().ToLowerInvariant(), out var role) ? role : ChatMessageRole.assistant,
            response.ToString(), 
            DateTime.UtcNow);
        chatSession.Messages.Add(chatMessageResponse);
        _context.ChatMessages.Add(chatMessageResponse);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ChatMessageDto>(chatMessage);
    }

    private static void GuardAgainstSessionNotFound(DbSet<ChatSessionEntity> dbSet, Guid sessionId)
    {
        if (sessionId != Guid.Empty && !dbSet.Any(x => x.Id == sessionId))
            throw new CustomValidationException(
            [
                new("ChatSessionId", "Chat Session does not exist")
            ]);
    }

    private static void GuardAgainstEmptyMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new CustomValidationException(
            [
                new("Message", "A message is required as a prompt to get an AI response")
            ]);
    }

    private static void GuardAgainstIdExsits(DbSet<ChatMessageEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
    }
}