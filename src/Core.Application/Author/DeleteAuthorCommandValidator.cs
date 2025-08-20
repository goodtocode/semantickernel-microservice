namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class DeleteAuthorCommandValidator : Validator<DeleteAuthorCommand>
{
    public DeleteAuthorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}