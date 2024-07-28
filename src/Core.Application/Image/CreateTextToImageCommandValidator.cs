namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class CreateTextToImageCommandValidator : AbstractValidator<CreateTextToImageCommand>
{
    public CreateTextToImageCommandValidator()
    {
        RuleFor(x => x.Prompt).NotEmpty();
    }
}