using FluentValidation.Results;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using AutoMapper.QueryableExtensions;
using Goodtocode.DotNet.Extensions;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatSessionCommand : IRequest<ChatSessionDto>
{
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
        
        ChatHistory chatHistory = new();
        chatHistory.AddUserMessage(request.Message);
        var response = await _chatService.GetChatMessageContentAsync(chatHistory, null, null, cancellationToken);
        // Persist chat session
        var chatSession = new ChatSessionEntity();
        chatSession.Messages.Add(new ChatMessageEntity()
        { 
            Content = request.Message, 
            Role = ChatMessageRole.User, 
            Timestamp = DateTime.UtcNow
        });
        _context.ChatSessions.Add(chatSession);
        _context.SaveChangesAsync(cancellationToken);

        // Return session

        //Id, chatcmpl-9Te5QEaE2fBhxt1mtHamj7U25NIRz
        //{ [Created, { 5/27/2024 11:30:32 PM +00:00}]}
        //ModelId "gpt-3.5-turbo" string
        //Role    { assistant}
        //Microsoft.SemanticKernel.ChatCompletion.AuthorRole
        //Content "There are 25 letters in the sentence \"hi, how many letters in this sentence?\""

        return chatSession.CopyPropertiesSafe<ChatSessionDto>();


        //var weatherChatCompletion = _context.ChatCompletions.Find(request.Key);
        //GuardAgainstWeatherChatCompletionNotFound(weatherChatCompletion);
        //var weatherChatCompletionValue = ChatCompletionValue.Create(request.Key, request.Date, (int)request.TemperatureF, request.Zipcodes);
        //if (weatherChatCompletionValue.IsFailure)
        //    throw new Exception(weatherChatCompletionValue.Error);
        //_context.ChatCompletions.Add(new ChatCompletion(weatherChatCompletionValue.Value));
        //await _context.SaveChangesAsync(CancellationToken.None);
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

//// Example usage
//public class ChatService
//{
//    private readonly ChatCompletionContext _dbContext;

//    public ChatService(ChatCompletionContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public ChatHistory LoadChatHistory(Guid sessionKey)
//    {
//        var chatHistory = new ChatHistory();
//        var messages = _dbContext.ChatMessages
//            .Where(m => m.Key == sessionKey)
//            .OrderBy(m => m.Timestamp)
//            .ToList();

//        foreach (var message in messages)
//        {
//            chatHistory.AddUserMessage(message.Content);
//            // Add other relevant info (e.g., system messages, assistant replies)
//        }

//        return chatHistory;
//    }

//    public void AddUserMessage(string sessionId, string content)
//    {
//        // Save the user message to the database
//        var message = new ChatMessage { ChatSessionId = sessionId, Content = content };
//        _dbContext.ChatMessages.Add(message);
//        _dbContext.SaveChanges();
//    }
//}