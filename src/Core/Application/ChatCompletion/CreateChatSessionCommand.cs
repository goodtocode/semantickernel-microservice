using FluentValidation.Results;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using AutoMapper.QueryableExtensions;
using Goodtocode.DotNet.Extensions;
using AutoMapper;
using CSharpFunctionalExtensions;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatSessionCommand : IRequest<ChatSessionDto>
{
    public Guid Key { get; set; }
    public string? Message { get; set; }
}

public class CreateChatSessionCommandHandler : IRequestHandler<CreateChatSessionCommand, ChatSessionDto>
{
    private IChatCompletionService _chatService;
    private IMapper _mapper;
    private readonly IChatCompletionContext _context;

    public CreateChatSessionCommandHandler(IChatCompletionService chatService, IChatCompletionContext context, IMapper mapper)
    {
        _context = context;
        _chatService = chatService;
        _mapper = mapper;
    }

    public async Task<ChatSessionDto> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
    {

        GuardAgainstEmptyMessage(request?.Message);
        
        // Get response
        ChatHistory chatHistory = new();
        chatHistory.AddUserMessage(request!.Message!);
        var response = await _chatService.GetChatMessageContentAsync(chatHistory, null, null, cancellationToken);
        
        // Persist chat session
        var chatSession = new ChatSessionEntity() { Key = request.Key };
        chatSession.Messages.Add(new ChatMessageEntity()
        { 
            Content = request!.Message!, 
            Role = ChatMessageRole.User, 
            Timestamp = DateTime.UtcNow
        });
        _context.ChatSessions.Add(chatSession);
        await _context.SaveChangesAsync(cancellationToken);

        // Return session
        return _mapper.Map<ChatSessionDto>(chatSession);
    }

    private static void GuardAgainstEmptyMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new CustomValidationException(new List<ValidationFailure>
            {
                new("Message", "A message is required to get a response")
            });
    }
}