using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class GetTextAudioPaginatedQuery : IRequest<PaginatedList<TextAudioDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTextAudioPaginatedQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetTextAudioPaginatedQuery, PaginatedList<TextAudioDto>>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<PaginatedList<TextAudioDto>> Handle(GetTextAudioPaginatedQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.TextAudio
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .Select(x => TextAudioDto.CreateFrom(x))
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return returnData;
    }
}