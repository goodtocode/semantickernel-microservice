namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class DeleteActorCommandValidator : Validator<DeleteActorCommand>
{
    public DeleteActorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}