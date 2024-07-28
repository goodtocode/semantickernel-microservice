using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class DeleteTextPromptCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteTextPromptCommandHandler(ISemanticKernelContext context) : IRequestHandler<DeleteTextPromptCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(DeleteTextPromptCommand request, CancellationToken cancellationToken)
    {
        var textPrompt = _context.TextPrompts.Find(request.Id);
        GuardAgainstNotFound(textPrompt);

        _context.TextPrompts.Remove(textPrompt!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(TextPromptEntity? textPrompt)
    {
        if (textPrompt == null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}