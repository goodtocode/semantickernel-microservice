using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class GetActorQuery : IRequest<ActorDto>
{
    public Guid ActorId { get; set; }
}

public class GetAuthorQueryHandler(ISemanticKernelContext context) : IRequestHandler<GetActorQuery, ActorDto>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ActorDto> Handle(GetActorQuery request, CancellationToken cancellationToken)
    {
        var actor = await _context.Actors.FindAsync([request.ActorId, cancellationToken], cancellationToken: cancellationToken);
        GuardAgainstNotFound(actor);

        return ActorDto.CreateFrom(actor);
    }

    private static void GuardAgainstNotFound(ActorEntity? Actor)
    {
        if (Actor == null)
            throw new CustomNotFoundException("Actor Not Found");
    }
}