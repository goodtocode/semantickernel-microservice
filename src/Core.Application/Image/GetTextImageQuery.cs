using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Image;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class GetTextImageQuery : IRequest<TextImageDto>
{
    public Guid Id { get; set; }
}

public class GetTextImageQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetTextImageQuery, TextImageDto>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<TextImageDto> Handle(GetTextImageQuery request,
                                CancellationToken cancellationToken)
    {
        var textImage = await _context.TextImages.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);
        GuardAgainstNotFound(textImage);

        return _mapper.Map<TextImageDto>(textImage);
    }

    private static void GuardAgainstNotFound(TextImageEntity? textImage)
    {
        if (textImage == null)
            throw new CustomNotFoundException("Text Image Not Found");
    }
}