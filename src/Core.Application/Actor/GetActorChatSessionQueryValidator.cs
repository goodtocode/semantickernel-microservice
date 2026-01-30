namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class GetActorChatSessionQueryValidator : Validator<GetActorChatSessionQuery>
{
    public GetActorChatSessionQueryValidator()
    {
        RuleFor(x => x.ActorId).NotEmpty();
        RuleFor(x => x.ChatSessionId).NotEmpty();
    }
}