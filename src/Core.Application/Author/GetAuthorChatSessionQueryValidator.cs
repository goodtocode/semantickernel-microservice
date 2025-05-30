namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class GetAuthorChatSessionQueryValidator : AbstractValidator<GetAuthorChatSessionQuery>
{
    public GetAuthorChatSessionQueryValidator()
    {
        RuleFor(x => x.AuthorId).NotEmpty();
        RuleFor(x => x.ChatSessionId).NotEmpty();
    }
}