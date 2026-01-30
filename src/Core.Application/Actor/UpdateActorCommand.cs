using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class UpdateActorCommand : IRequest
{
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UpdateAuthorCommandHandler(ISemanticKernelContext context) : IRequestHandler<UpdateActorCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstIdEmpty(request.OwnerId);
        var actor = await _context.Actors.Where(x => x.OwnerId == request.OwnerId).FirstOrDefaultAsync(cancellationToken);
        GuardAgainstNotFound(actor);

        _context.Actors.Update(actor!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstIdEmpty(Guid ownerId)
    {
        if (ownerId == Guid.Empty)
            throw new CustomValidationException(
            [
                new("OwnerId", "A valid OwnerId is required to update an actor")
            ]);
    }

    private static void GuardAgainstNotFound(ActorEntity? actor)
    {
        if (actor == null)
            throw new CustomNotFoundException("Actor Not Found");
    }
}