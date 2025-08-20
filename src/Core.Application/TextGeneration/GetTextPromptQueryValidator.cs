namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class GetTextPromptQueryValidator : Validator<GetTextPromptQuery>
{
    public GetTextPromptQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}