namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class GetActorChatSessionsQueryValidator : Validator<GetActorChatSessionsQuery>
{
    public GetActorChatSessionsQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();

        RuleFor(v => v.StartDate).NotEmpty()
            .When(v => v.EndDate != null)
            .LessThanOrEqualTo(v => v.EndDate);

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .When(v => v.StartDate != null)
            .GreaterThanOrEqualTo(v => v.StartDate);
    }
}