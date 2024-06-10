using AutoMapper.QueryableExtensions;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.ForecastLists.Queries.GetAll;

namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatSessionQuery : IRequest<ChatSessionDto>
{
    public Guid Key { get; set; }
}

public class GetChatSessionQueryHandler : IRequestHandler<GetChatSessionQuery, ChatSessionDto>
{
    private IChatCompletionContext _context;
    private IMapper _mapper;

    public GetChatSessionQueryHandler(IChatCompletionContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ChatSessionDto> Handle(GetChatSessionQuery request,
                                CancellationToken cancellationToken)
    {
        return await _context.ChatMessages
            .ProjectTo<ChatSessionDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Key == request.Key, cancellationToken);
    }
}