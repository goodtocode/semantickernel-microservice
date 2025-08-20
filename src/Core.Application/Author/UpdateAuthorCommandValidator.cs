namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class UpdateAuthorCommandValidator : Validator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}