namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatSessionQueryValidator : AbstractValidator<GetChatSessionQuery>
{
    public GetChatSessionQueryValidator()
    {
        RuleFor(x => x.Key).NotEmpty();
    }
}