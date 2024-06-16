using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatSessionQuery : IRequest<ChatSessionDto>
{
    public Guid Key { get; set; }
}

public class GetChatSessionQueryHandler : IRequestHandler<GetChatSessionQuery, ChatSessionDto>
{
    private IChatCompletionContext _context;
    private IMapper _mapper;

    public GetChatSessionQueryHandler(IChatCompletionContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ChatSessionDto> Handle(GetChatSessionQuery request,
                                CancellationToken cancellationToken)
    {
        var chatSession = await _context.ChatSessions.FindAsync(request.Key);
        GuardAgainstForecastNotFound(chatSession);

        return _mapper.Map<ChatSessionDto>(chatSession);
    }

    private static void GuardAgainstForecastNotFound(ChatSessionEntity? chatSession)
    {
        if (chatSession == null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}