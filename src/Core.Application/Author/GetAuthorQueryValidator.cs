namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class GetAuthorQueryValidator : Validator<GetAuthorQuery>
{
    public GetAuthorQueryValidator()
    {
        RuleFor(x => x.AuthorId).NotEmpty();
    }
}