namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class AddTextGenerationCommandValidator : AbstractValidator<CreateTextGenerationCommand>
{
    public AddTextGenerationCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty();
    }
}