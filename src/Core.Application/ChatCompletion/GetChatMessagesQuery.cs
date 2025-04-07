using AutoMapper.QueryableExtensions;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatMessagesQuery : IRequest<ICollection<ChatMessageDto>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetChatMessagesQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetChatMessagesQuery, ICollection<ChatMessageDto>>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<ICollection<ChatMessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var returnData = await _context.ChatMessages
            .OrderByDescending(x => x.Timestamp)
            .Where(x => (request.StartDate == null || x.Timestamp > request.StartDate)
                    && (request.EndDate == null || x.Timestamp < request.EndDate))
            .ProjectTo<ChatMessageDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return returnData;
    }
}