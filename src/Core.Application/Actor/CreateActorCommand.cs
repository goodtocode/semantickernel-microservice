using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class CreateActorCommand : IRequest<ActorDto>
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public Guid OwnerId { get; set; }
    public Guid TenantId { get; set; }
}

public class CreateAuthorCommandHandler(ISemanticKernelContext context) : IRequestHandler<CreateActorCommand, ActorDto>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ActorDto> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstEmptyOwnerId(request?.OwnerId);
        GuardAgainstIdExists(_context.Actors, request!.Id);

        var Actor = ActorEntity.Create(request!.Id == Guid.Empty ? Guid.NewGuid() : request!.Id, request.OwnerId, request.TenantId, request.FirstName, request.LastName, request.Email);
        _context.Actors.Add(Actor);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
        }

        return ActorDto.CreateFrom(Actor);
    }

    private static void GuardAgainstEmptyOwnerId(Guid? ownerId)
    {
        if (ownerId == Guid.Empty)
            throw new CustomValidationException(
            [
                new("OwnerId", "A OwnerId is required to link an actor with an account")
            ]);
    }

    private static void GuardAgainstIdExists(DbSet<ActorEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomConflictException("Id already exists");
    }
}