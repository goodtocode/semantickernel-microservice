namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class GetAuthorQueryValidator : AbstractValidator<GetAuthorQuery>
{
    public GetAuthorQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}