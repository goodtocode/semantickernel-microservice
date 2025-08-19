namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class GetAuthorChatSessionQueryValidator : Validator<GetAuthorChatSessionQuery>
{
    public GetAuthorChatSessionQueryValidator()
    {
        RuleFor(x => x.AuthorId).NotEmpty();
        RuleFor(x => x.ChatSessionId).NotEmpty();
    }
}