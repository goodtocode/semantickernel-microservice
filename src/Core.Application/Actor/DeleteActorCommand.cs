using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class DeleteActorCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteAuthorCommandHandler(ISemanticKernelContext context) : IRequestHandler<DeleteActorCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(DeleteActorCommand request, CancellationToken cancellationToken)
    {
        var Actor = _context.Actors.Find(request.Id);
        GuardAgainstNotFound(Actor);

        _context.Actors.Remove(Actor!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(ActorEntity? Actor)
    {
        if (Actor == null)
            throw new CustomNotFoundException("Actor Not Found");
    }
}