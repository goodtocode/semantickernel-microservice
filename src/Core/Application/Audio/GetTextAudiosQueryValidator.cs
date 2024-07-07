namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class GetTextAudiosQueryValidator : AbstractValidator<GetTextAudiosQuery>
{
    public GetTextAudiosQueryValidator()
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