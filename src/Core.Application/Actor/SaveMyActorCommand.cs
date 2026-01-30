using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Domain.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Auth;

namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class SaveMyActorCommand : IRequest<ActorDto>, IUserInfoRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public Guid TenantId { get; set; }
    public IUserEntity? UserInfo { get; set; }
}

public class SaveAuthorCommandHandler(ISemanticKernelContext context) : IRequestHandler<SaveMyActorCommand, ActorDto>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task<ActorDto> Handle(SaveMyActorCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstEmptyTenantId(request?.TenantId);

        var actor = await _context.Actors.Where(x => x.OwnerId == request!.UserInfo!.OwnerId && x.TenantId == request.TenantId).FirstOrDefaultAsync(cancellationToken);
        if (actor is not null)
        {
            actor.Update(request?.FirstName, request?.LastName ?? actor.LastName, request?.Email);
            _context.Actors.Update(actor!);
        }
        else
        {
        actor = ActorEntity.Create(Guid.NewGuid(), request!.UserInfo!.OwnerId, request.TenantId, request.FirstName, request.LastName, request.Email);
            _context.Actors.Add(actor);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ActorDto.CreateFrom(actor);
    }

    private static void GuardAgainstEmptyTenantId(Guid? tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new CustomValidationException(
            [
                new("TenantId", "A TenantId is required to link an actor with an account")
            ]);
    }
}