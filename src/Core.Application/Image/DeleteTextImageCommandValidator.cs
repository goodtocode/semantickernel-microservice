namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class DeleteTextImageCommandValidator : Validator<DeleteTextImageCommand>
{
    public DeleteTextImageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}