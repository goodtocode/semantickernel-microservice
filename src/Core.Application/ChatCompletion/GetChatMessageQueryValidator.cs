namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class GetChatMessageQueryValidator : Validator<GetChatMessageQuery>
{
    public GetChatMessageQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}