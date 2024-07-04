using AutoMapper.QueryableExtensions;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class GetTextPromptsPaginatedQuery : IRequest<PaginatedList<TextPromptDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTextPromptsPaginatedQueryHandler : IRequestHandler<GetTextPromptsPaginatedQuery, PaginatedList<TextPromptDto>>
{
    private readonly ISemanticKernelContext _context;
    private readonly IMapper _mapper;

    public GetTextPromptsPaginatedQueryHandler(ISemanticKernelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<TextPromptDto>> Handle(GetTextPromptsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.TextPrompts
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .ProjectTo<TextPromptDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return returnData;
    }
}