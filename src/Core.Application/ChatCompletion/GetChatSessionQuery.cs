using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatSessionQuery : IRequest<ChatSessionDto>
{
    public Guid Id { get; set; }
}

public class GetChatSessionQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetChatSessionQuery, ChatSessionDto>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ChatSessionDto> Handle(GetChatSessionQuery request,
                                CancellationToken cancellationToken)
    {
        var chatSession = await _context.ChatSessions.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);
        GuardAgainstNotFound(chatSession);

        return _mapper.Map<ChatSessionDto>(chatSession);
    }

    private static void GuardAgainstNotFound(ChatSessionEntity? chatSession)
    {
        if (chatSession == null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}