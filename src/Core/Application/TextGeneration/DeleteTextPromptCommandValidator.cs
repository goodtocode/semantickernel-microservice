namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class DeleteTextPromptCommandValidator : AbstractValidator<DeleteTextPromptCommand>
{
    public DeleteTextPromptCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}