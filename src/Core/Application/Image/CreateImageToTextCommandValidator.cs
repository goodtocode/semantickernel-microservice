namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class CreateImageToTextCommandValidator : AbstractValidator<CreateImageToTextCommand>
{
    public CreateImageToTextCommandValidator()
    {
        RuleFor(x => x.ImageBytes)
            .NotNull()
            .When(x => x.ImageUrl == null);

        RuleFor(x => x.ImageUrl)
            .NotNull()
            .When(x => x.ImageBytes == null);
    }
}