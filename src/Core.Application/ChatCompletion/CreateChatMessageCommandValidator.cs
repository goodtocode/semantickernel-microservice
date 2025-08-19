namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatMessageCommandValidator : Validator<CreateChatMessageCommand>
{
    public CreateChatMessageCommandValidator()
    {
        RuleFor(x => x.ChatSessionId).NotEmpty();
        RuleFor(x => x.Message).NotEmpty();
    }
}