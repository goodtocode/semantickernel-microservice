using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class GetAuthorChatSessionsQuery : IRequest<ICollection<ChatSessionDto>>
{
    public Guid AuthorId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetAuthorChatSessionsQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetAuthorChatSessionsQuery, ICollection<ChatSessionDto>>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ICollection<ChatSessionDto>> Handle(GetAuthorChatSessionsQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.ChatSessions
            .OrderByDescending(x => x.Timestamp)
            .Where(x => x.AuthorId == request.AuthorId
                    && (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .Select(x => ChatSessionDto.CreateFrom(x))
            .ToListAsync(cancellationToken);

        return returnData;
    }
}