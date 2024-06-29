namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
{
    public DeleteAuthorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}