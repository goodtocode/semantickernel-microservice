using AutoMapper.QueryableExtensions;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class GetTextPromptsQuery : IRequest<ICollection<TextPromptDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetTextPromptsQueryHandler : IRequestHandler<GetTextPromptsQuery, ICollection<TextPromptDto>>
{
    private readonly ISemanticKernelContext _context;
    private readonly IMapper _mapper;

    public GetTextPromptsQueryHandler(ISemanticKernelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ICollection<TextPromptDto>> Handle(GetTextPromptsQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.TextPrompts
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .ProjectTo<TextPromptDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return returnData;
    }
}