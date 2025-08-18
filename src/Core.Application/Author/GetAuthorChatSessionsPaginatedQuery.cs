using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class GetAuthorChatSessionsPaginatedQuery : IRequest<PaginatedList<ChatSessionDto>>
{
    public Guid AuthorId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAuthorChatSessionsPaginatedQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetAuthorChatSessionsPaginatedQuery, PaginatedList<ChatSessionDto>>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<PaginatedList<ChatSessionDto>> Handle(GetAuthorChatSessionsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.ChatSessions
        .OrderByDescending(x => x.Timestamp)
        .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
            && (request.EndDate == null || x.Timestamp < request.EndDate))
        .Select(x => ChatSessionDto.CreateFrom(x))
        .PaginatedListAsync(request.PageNumber, request.PageSize);

        return returnData;

    }
}