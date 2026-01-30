namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class UpdateActorCommandValidator : Validator<UpdateActorCommand>
{
    public UpdateActorCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}