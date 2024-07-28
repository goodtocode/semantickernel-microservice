namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class CreateTextPromptCommandValidator : AbstractValidator<CreateTextPromptCommand>
{
    public CreateTextPromptCommandValidator()
    {
        RuleFor(x => x.Prompt).NotEmpty();
    }
}