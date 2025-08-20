namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatSessionsPaginatedQueryValidator : Validator<GetChatSessionsPaginatedQuery>
{
    public GetChatSessionsPaginatedQueryValidator()
    {
        RuleFor(v => v.StartDate).NotEmpty()
            .When(v => v.EndDate != null)
            .LessThanOrEqualTo(v => v.EndDate);

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .When(v => v.StartDate != null)
            .GreaterThanOrEqualTo(v => v.StartDate);

        RuleFor(x => x.PageNumber).NotEqual(0);

        RuleFor(x => x.PageSize).NotEqual(0);
    }
}