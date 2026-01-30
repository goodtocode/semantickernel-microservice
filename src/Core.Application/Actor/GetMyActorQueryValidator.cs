namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class GetMyActorQueryValidator : Validator<GetMyActorQuery>
{
    public GetMyActorQueryValidator()
    {
        RuleFor(x => x.UserInfo).NotEmpty();
    }
}