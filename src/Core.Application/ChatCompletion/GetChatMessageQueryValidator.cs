namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatMessageQueryValidator : AbstractValidator<GetChatMessageQuery>
{
    public GetChatMessageQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}