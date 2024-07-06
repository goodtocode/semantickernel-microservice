using AutoMapper.QueryableExtensions;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class GetTextImagesQuery : IRequest<ICollection<TextImageDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetTextImagesQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetTextImagesQuery, ICollection<TextImageDto>>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ICollection<TextImageDto>> Handle(GetTextImagesQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.TextImages
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .ProjectTo<TextImageDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return returnData;
    }
}