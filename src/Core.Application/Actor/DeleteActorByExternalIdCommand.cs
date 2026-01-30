using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class DeleteActorByOwnerIdCommand : IRequest
{
    public Guid OwnerId { get; set; }
}

public class DeleteAuthorByOwnerIdCommandHandler(ISemanticKernelContext context) : IRequestHandler<DeleteActorByOwnerIdCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(DeleteActorByOwnerIdCommand request, CancellationToken cancellationToken)
    {
        var actor = await _context.Actors.Where(x => x.OwnerId == request.OwnerId).FirstOrDefaultAsync(cancellationToken);
        GuardAgainstNotFound(actor);

        _context.Actors.Remove(actor!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(ActorEntity? Actor)
    {
        if (Actor == null)
            throw new CustomNotFoundException("Actor Not Found");
    }
}