using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatSessionCommand : IRequest<ChatSessionDto>
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
}

public class CreateChatSessionCommandHandler(Kernel kernel, ISemanticKernelContext context) : IRequestHandler<CreateChatSessionCommand, ChatSessionDto>
{
    private readonly Kernel _kernel = kernel;
    private readonly ISemanticKernelContext _context = context;

    public async Task<ChatSessionDto> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstMissingAuthor(request.AuthorId);
        GuardAgainstEmptyMessage(request?.Message);
        GuardAgainstIdExsits(_context.ChatSessions, request!.Id);

        var service = _kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chatHistory = [];
        chatHistory.AddUserMessage(request!.Message!);
        var executionSettings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        var response = await service.GetChatMessageContentAsync(chatHistory, executionSettings, _kernel, cancellationToken);

        var author = await _context.Authors
            .FirstOrDefaultAsync(x => x.Id == request.AuthorId, cancellationToken);
        if (author == null)
        {
            author = AuthorEntity.Create(request.AuthorId, request?.AuthorName);
            _context.Authors.Add(author);
        }
        var title = request!.Title ?? $"{request!.Message![..(request.Message!.Length >= 25 ? 25 : request.Message!.Length)]}";
        var chatSession = ChatSessionEntity.Create(
            request.Id,
            request.AuthorId,
            title,            
            Enum.TryParse<ChatMessageRole>(response.Role.ToString().ToLowerInvariant(), out var role) ? role : ChatMessageRole.assistant,
            request.Message!,
            response.ToString()
        );
        _context.ChatSessions.Add(chatSession);
        await _context.SaveChangesAsync(cancellationToken);

        return ChatSessionDto.CreateFrom(chatSession);
    }

    private static void GuardAgainstMissingAuthor(Guid authorId)
    {
        if (authorId == Guid.Empty)
            throw new CustomValidationException(
            [
                new("AuthorId", "AuthorId required for sessions")
            ]);
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