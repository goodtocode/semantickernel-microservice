namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class CreateActorCommandValidator : Validator<CreateActorCommand>
{
    public CreateActorCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
    }
}