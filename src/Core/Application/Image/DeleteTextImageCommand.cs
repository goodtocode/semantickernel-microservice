using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Image;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class DeleteTextImageCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteTextImageCommandHandler(ISemanticKernelContext context) : IRequestHandler<DeleteTextImageCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(DeleteTextImageCommand request, CancellationToken cancellationToken)
    {
        var TextImage = _context.TextImages.Find(request.Id);
        GuardAgainstNotFound(TextImage);

        _context.TextImages.Remove(TextImage!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(TextImageEntity? TextImage)
    {
        if (TextImage == null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}