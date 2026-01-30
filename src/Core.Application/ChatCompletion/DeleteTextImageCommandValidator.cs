namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class DeleteTextImageCommandValidator : Validator<DeleteTextImageCommand>
{
    public DeleteTextImageCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}