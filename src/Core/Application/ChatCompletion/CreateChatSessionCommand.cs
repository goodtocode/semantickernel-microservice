using FluentValidation.Results;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Net.NetworkInformation;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatSessionCommand : IRequest<ChatSessionDto>
{
    public Guid Key { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
}

public class CreateChatSessionCommandHandler(IChatCompletionService chatService, IChatCompletionContext context, IMapper mapper) : IRequestHandler<CreateChatSessionCommand, ChatSessionDto>
{
    private readonly IChatCompletionService _chatService = chatService;
    private readonly IMapper _mapper = mapper;
    private readonly IChatCompletionContext _context = context;

    public async Task<ChatSessionDto> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
    {
        
        GuardAgainstEmptyMessage(request?.Message);
        
        // Get response
        ChatHistory chatHistory = [];
        chatHistory.AddUserMessage(request!.Message!);
        var response = await _chatService.GetChatMessageContentAsync(chatHistory, null, null, cancellationToken);

        // Persist chat session
        var chatSession = new ChatSessionEntity() { Key = request.Key == Guid.Empty ? Guid.NewGuid() : request.Key };
        chatSession.Messages.Add(new ChatMessageEntity()
        {
            Content = request!.Message!,
            Role = ChatMessageRole.user,
            Timestamp = DateTime.UtcNow
        });
        chatSession.Messages.Add(new ChatMessageEntity()
        {
            Content = response.ToString(),
            Role = Enum.Parse<ChatMessageRole>(response.Role.ToString()),
            Timestamp = DateTime.UtcNow
        });
        _context.ChatSessions.Add(chatSession);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new CustomValidationException(
            [
                new("Key", "Key already exists")
            ]);
        }

        // Return session
        ChatSessionDto returnValue;
        try
        {
            returnValue = _mapper.Map<ChatSessionDto>(chatSession);
        }
        catch (Exception)
        {
            throw new CustomValidationException(
            [
                new("Key", "Key already exists")
            ]);
        }
        return returnValue;
    }

    private static void GuardAgainstEmptyMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new CustomValidationException(
            [
                new("Message", "A message is required to get a response")
            ]);
    }
}