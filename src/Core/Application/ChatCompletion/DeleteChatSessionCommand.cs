using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class DeleteChatSessionCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteChatSessionCommandHandler(ISemanticKernelContext context) : IRequestHandler<DeleteChatSessionCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(DeleteChatSessionCommand request, CancellationToken cancellationToken)
    {
        var chatSession = _context.ChatSessions.Find(request.Id);
        GuardAgainstNotFound(chatSession);

        _context.ChatSessions.Remove(chatSession!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(ChatSessionEntity? chatSession)
    {
        if (chatSession == null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}