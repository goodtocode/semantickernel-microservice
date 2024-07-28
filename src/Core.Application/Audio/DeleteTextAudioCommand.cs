using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Audio;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class DeleteTextAudioCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteTextAudioCommandHandler(ISemanticKernelContext context) : IRequestHandler<DeleteTextAudioCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(DeleteTextAudioCommand request, CancellationToken cancellationToken)
    {
        var textAudio = _context.TextAudio.Find(request.Id);
        GuardAgainstNotFound(textAudio);

        _context.TextAudio.Remove(textAudio!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(TextAudioEntity? textAudio)
    {
        if (textAudio == null)
            throw new CustomNotFoundException("Text Audio Not Found");
    }
}