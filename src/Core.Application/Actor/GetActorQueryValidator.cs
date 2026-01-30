namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class GetActorQueryValidator : Validator<GetActorQuery>
{
    public GetActorQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
    }
}