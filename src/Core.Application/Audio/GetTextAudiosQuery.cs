using Goodtocode.SemanticKernel.Core.Application.Abstractions;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class GetTextAudiosQuery : IRequest<ICollection<TextAudioDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetTextAudiosQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetTextAudiosQuery, ICollection<TextAudioDto>>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ICollection<TextAudioDto>> Handle(GetTextAudiosQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.TextAudio
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .Select(x => TextAudioDto.CreateFrom(x))
            .ToListAsync(cancellationToken);

        return returnData;
    }
}