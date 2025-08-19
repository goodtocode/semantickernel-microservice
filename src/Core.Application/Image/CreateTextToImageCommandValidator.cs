namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class CreateTextToImageCommandValidator : Validator<CreateTextToImageCommand>
{
    public CreateTextToImageCommandValidator()
    {
        RuleFor(x => x.Prompt).NotEmpty();
    }
}