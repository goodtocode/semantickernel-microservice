using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Audio;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class GetTextAudioQuery : IRequest<TextAudioDto>
{
    public Guid Id { get; set; }
}

public class GetTextAudioQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetTextAudioQuery, TextAudioDto>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<TextAudioDto> Handle(GetTextAudioQuery request,
                                CancellationToken cancellationToken)
    {
        var textAudio = await _context.TextAudio.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);
        GuardAgainstNotFound(textAudio);

        return _mapper.Map<TextAudioDto>(textAudio);
    }

    private static void GuardAgainstNotFound(TextAudioEntity? textAudio)
    {
        if (textAudio == null)
            throw new CustomNotFoundException("Text Audio Not Found");
    }
}