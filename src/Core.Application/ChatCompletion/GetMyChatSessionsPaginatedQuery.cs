using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Core.Domain.Auth;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetMyChatSessionsPaginatedQuery : IRequest<PaginatedList<ChatSessionDto>>, IUserInfoRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public IUserEntity? UserInfo { get; set; }
}

public class GetAuthorChatSessionsByExternalIdPaginatedQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetMyChatSessionsPaginatedQuery, PaginatedList<ChatSessionDto>>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<PaginatedList<ChatSessionDto>> Handle(GetMyChatSessionsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.ChatSessions
            .Include(x => x.Messages)
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                && (request.EndDate == null || x.Timestamp < request.EndDate))
                        .Join(_context.Actors,
                  cs => cs.ActorId,
                  a => a.Id,
                  (cs, a) => new { ChatSession = cs, Actor = a })
            .Where(joined => joined.Actor.OwnerId == request.UserInfo!.OwnerId)
            .Select(joined => joined.ChatSession)
            .Select(x => ChatSessionDto.CreateFrom(x))        
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return returnData;

    }
}