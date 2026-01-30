using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Auth;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class GetMyActorQuery : IRequest<ActorDto>, IUserInfoRequest
{
    public IUserEntity? UserInfo { get; set; }
}

public class GetAuthorByOwnerIdQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetMyActorQuery, ActorDto>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ActorDto> Handle(GetMyActorQuery request, CancellationToken cancellationToken)
    {
        var actor = await _context.Actors.Where(x => x.OwnerId == request!.UserInfo!.OwnerId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        GuardAgainstNotFound(actor);

        return ActorDto.CreateFrom(actor);
    }

    private static void GuardAgainstNotFound(ActorEntity? Actor)
    {
        if (Actor == null)
            throw new CustomNotFoundException("Actor Not Found");
    }
}