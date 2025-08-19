using Goodtocode.SemanticKernel.Core.Application.Abstractions;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class GetTextImagesQuery : IRequest<ICollection<TextImageDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetTextImagesQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetTextImagesQuery, ICollection<TextImageDto>>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ICollection<TextImageDto>> Handle(GetTextImagesQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.TextImages
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .Select(x => TextImageDto.CreateFrom(x))
            .ToListAsync(cancellationToken);

        return returnData;
    }
}