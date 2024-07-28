using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class UpdateChatSessionCommand : IRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ICollection<ChatMessageDto> Messages { get; set; } = [];
}

public class UpdateChatSessionCommandHandler (ISemanticKernelContext context) : IRequestHandler<UpdateChatSessionCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(UpdateChatSessionCommand request, CancellationToken cancellationToken)
    {
        var chatSession = _context.ChatSessions.Find(request.Id);
        GuardAgainstNotFound(chatSession);


        _context.ChatSessions.Update(chatSession!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(ChatSessionEntity? chatSession)
    {
        if (chatSession == null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}