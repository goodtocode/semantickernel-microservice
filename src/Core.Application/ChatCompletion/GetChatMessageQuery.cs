using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatMessageQuery : IRequest<ChatMessageDto>
{
    public Guid Id { get; set; }
}

public class GetChatMessageQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetChatMessageQuery, ChatMessageDto>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ChatMessageDto> Handle(GetChatMessageQuery request,
                                CancellationToken cancellationToken)
    {
        var chatMessage = await _context.ChatMessages.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);
        GuardAgainstNotFound(chatMessage);

        return _mapper.Map<ChatMessageDto>(chatMessage);
    }

    private static void GuardAgainstNotFound(ChatMessageEntity? chatMessage)
    {
        if (chatMessage == null)
            throw new CustomNotFoundException("Chat Message Not Found");
    }
}