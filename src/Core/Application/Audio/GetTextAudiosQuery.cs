using AutoMapper.QueryableExtensions;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class GetTextAudiosQuery : IRequest<ICollection<TextAudioDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetTextAudiosQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetTextAudiosQuery, ICollection<TextAudioDto>>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ICollection<TextAudioDto>> Handle(GetTextAudiosQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.TextAudio
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .ProjectTo<TextAudioDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return returnData;
    }
}