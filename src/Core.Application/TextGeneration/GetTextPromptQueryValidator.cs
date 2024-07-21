namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class GetTextPromptQueryValidator : AbstractValidator<GetTextPromptQuery>
{
    public GetTextPromptQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}