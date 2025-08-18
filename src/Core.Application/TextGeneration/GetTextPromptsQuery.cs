using Goodtocode.SemanticKernel.Core.Application.Abstractions;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class GetTextPromptsQuery : IRequest<ICollection<TextPromptDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetTextPromptsQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetTextPromptsQuery, ICollection<TextPromptDto>>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ICollection<TextPromptDto>> Handle(GetTextPromptsQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.TextPrompts
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .Select(x => TextPromptDto.CreateFrom(x))
            .ToListAsync(cancellationToken);

        return returnData;
    }
}