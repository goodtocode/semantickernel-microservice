using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatSessionCommand : IRequest<ChatSessionDto>
{
    public Guid Id { get; set; }
    public Guid ActorId { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
}

public class CreateChatSessionCommandHandler(Kernel kernel, ISemanticKernelContext context) : IRequestHandler<CreateChatSessionCommand, ChatSessionDto>
{
    private readonly Kernel _kernel = kernel;
    private readonly ISemanticKernelContext _context = context;

    public async Task<ChatSessionDto> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstEmtpyActorId(request.ActorId);
        GuardAgainstEmptyMessage(request?.Message);
        GuardAgainstIdExists(_context.ChatSessions, request!.Id);

        var service = _kernel.GetRequiredService<IChatCompletionService>();
        ChatHistory chatHistory = [];
        chatHistory.AddUserMessage(request!.Message!);
        var executionSettings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        var response = await service.GetChatMessageContentAsync(chatHistory, executionSettings, _kernel, cancellationToken);

        var actor = await _context.Actors
            .FirstOrDefaultAsync(x => x.OwnerId == request.ActorId, cancellationToken);
        GuardAgainstActorNotFound(actor);

        var title = request!.Title ?? $"{request!.Message![..(request.Message!.Length >= 25 ? 25 : request.Message!.Length)]}";
        var chatSession = ChatSessionEntity.Create(
            request.Id,
            actor!.Id,
            title,
            Enum.TryParse<ChatMessageRole>(response.Role.ToString().ToLowerInvariant(), out var role) ? role : ChatMessageRole.assistant,
            request.Message!,
            response.ToString()
        );
        _context.ChatSessions.Add(chatSession);
        await _context.SaveChangesAsync(cancellationToken);

        return ChatSessionDto.CreateFrom(chatSession);
    }

    private static void GuardAgainstActorNotFound(ActorEntity? actor)
    {
        if (actor == null)
            throw new CustomValidationException(
            [
                new("ActorId", "ActorId required for sessions")
            ]);
    }

    private static void GuardAgainstEmtpyActorId(Guid actorId)
    {
        if (actorId == Guid.Empty)
            throw new CustomValidationException(
            [
                new("ActorId", "ActorId required for sessions")
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

    private static void GuardAgainstIdExists(DbSet<ChatSessionEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomConflictException("Id already exists");
    }
}