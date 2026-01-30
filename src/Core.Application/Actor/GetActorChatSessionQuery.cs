using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class GetActorChatSessionQuery : IRequest<ChatSessionDto>
{
    public Guid ActorId { get; set; }
    public Guid ChatSessionId { get; set; }
}

public class GetAuthorChatSessionQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetActorChatSessionQuery, ChatSessionDto>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ChatSessionDto> Handle(GetActorChatSessionQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.ChatSessions
            .FirstOrDefaultAsync(x => x.Id == request.ChatSessionId && x.ActorId == request.ActorId, cancellationToken: cancellationToken);
        GuardAgainstNotFound(returnData);

        return ChatSessionDto.CreateFrom(returnData);
    }

    private static void GuardAgainstNotFound(ChatSessionEntity? entity)
    {
        if (entity is null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}