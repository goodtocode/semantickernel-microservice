using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatSessionCommand : IRequest<ChatSessionDto>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
}

public class CreateChatSessionCommandHandler(IChatCompletionService chatService, ISemanticKernelContext context, IMapper mapper) : IRequestHandler<CreateChatSessionCommand, ChatSessionDto>
{
    private readonly IChatCompletionService _chatService = chatService;
    private readonly IMapper _mapper = mapper;
    private readonly ISemanticKernelContext _context = context;

    public async Task<ChatSessionDto> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstEmptyMessage(request?.Message);
        GuardAgainstIdExsits(_context.ChatSessions, request!.Id);

        // Get response
        ChatHistory chatHistory = [];
        chatHistory.AddUserMessage(request!.Message!);
        var response = await _chatService.GetChatMessageContentAsync(chatHistory, null, null, cancellationToken);

        // Persist chat session
        var chatSession = new ChatSessionEntity() { Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id };
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
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ChatSessionDto>(chatSession);
    }

    private static void GuardAgainstEmptyMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new CustomValidationException(
            [
                new("Message", "A message is required to get a response")
            ]);
    }

    private static void GuardAgainstIdExsits(DbSet<ChatSessionEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
    }
}