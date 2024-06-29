using AutoMapper.QueryableExtensions;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetAuthorChatSessionsQuery : IRequest<ICollection<ChatSessionDto>>
{
    public Guid AuthorId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetAuthorChatSessionsQueryHandler : IRequestHandler<GetAuthorChatSessionsQuery, ICollection<ChatSessionDto>>
{
    private readonly ISemanticKernelContext _context;
    private readonly IMapper _mapper;

    public GetAuthorChatSessionsQueryHandler(ISemanticKernelContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ICollection<ChatSessionDto>> Handle(GetAuthorChatSessionsQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.ChatSessions
            .OrderByDescending(x => x.Timestamp)
            .Where(x => x.AuthorId == request.AuthorId &&
                    ((request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate)))
            .ProjectTo<ChatSessionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return returnData;
    }
}