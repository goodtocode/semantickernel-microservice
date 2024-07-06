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
        var textImage = _context.TextImages.Find(request.Id);
        GuardAgainstNotFound(textImage);

        _context.TextImages.Remove(textImage!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(TextImageEntity? textImage)
    {
        if (textImage == null)
            throw new CustomNotFoundException("Text Image Not Found");
    }
}