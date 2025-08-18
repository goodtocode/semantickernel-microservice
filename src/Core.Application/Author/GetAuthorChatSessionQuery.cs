using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class GetAuthorChatSessionQuery : IRequest<ChatSessionDto>
{
    public Guid AuthorId { get; set; }
    public Guid ChatSessionId { get; set; }
}

public class GetAuthorChatSessionQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetAuthorChatSessionQuery, ChatSessionDto>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ChatSessionDto> Handle(GetAuthorChatSessionQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.ChatSessions
            .FirstOrDefaultAsync(x => x.Id == request.ChatSessionId && x.AuthorId == request.AuthorId, cancellationToken: cancellationToken);
        GuardAgainstNotFound(returnData);

        return ChatSessionDto.CreateFrom(returnData);
    }

    private static void GuardAgainstNotFound(ChatSessionEntity? entity)
    {
        if (entity is null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}