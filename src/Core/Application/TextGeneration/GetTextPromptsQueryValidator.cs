namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class GetTextPromptsQueryValidator : AbstractValidator<GetTextPromptsQuery>
{
    public GetTextPromptsQueryValidator()
    {
        RuleFor(v => v.StartDate).NotEmpty()
            .When(v => v.EndDate != null)
            .LessThanOrEqualTo(v => v.EndDate);

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .When(v => v.StartDate != null)
            .GreaterThanOrEqualTo(v => v.StartDate);
    }
}