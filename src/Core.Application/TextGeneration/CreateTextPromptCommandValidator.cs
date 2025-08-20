namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class CreateTextPromptCommandValidator : Validator<CreateTextPromptCommand>
{
    public CreateTextPromptCommandValidator()
    {
        RuleFor(x => x.Prompt).NotEmpty();
    }
}