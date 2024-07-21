using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class GetTextPromptQuery : IRequest<TextPromptDto>
{
    public Guid Id { get; set; }
}

public class GetTextPromptQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetTextPromptQuery, TextPromptDto>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<TextPromptDto> Handle(GetTextPromptQuery request,
                                CancellationToken cancellationToken)
    {
        var textPrompt = await _context.TextPrompts.FindAsync([request.Id, cancellationToken], cancellationToken: cancellationToken);
        GuardAgainstNotFound(textPrompt);

        return _mapper.Map<TextPromptDto>(textPrompt);
    }

    private static void GuardAgainstNotFound(TextPromptEntity? textPrompt)
    {
        if (textPrompt == null)
            throw new CustomNotFoundException("Chat Session Not Found");
    }
}