namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetMyChatSessionsQueryValidator : Validator<GetMyChatSessionsQuery>
{
    public GetMyChatSessionsQueryValidator()
    {
        RuleFor(x => x.UserInfo).NotEmpty();

        RuleFor(v => v.StartDate).NotEmpty()
            .When(v => v.EndDate != null)
            .LessThanOrEqualTo(v => v.EndDate);

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .When(v => v.StartDate != null)
            .GreaterThanOrEqualTo(v => v.StartDate);
    }
}