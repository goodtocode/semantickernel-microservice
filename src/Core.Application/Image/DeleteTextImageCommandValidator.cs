namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class DeleteTextImageCommandValidator : AbstractValidator<DeleteTextImageCommand>
{
    public DeleteTextImageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}