namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatMessageCommandValidator : AbstractValidator<CreateChatMessageCommand>
{
    public CreateChatMessageCommandValidator()
    {
        RuleFor(x => x.ChatSessionId).NotEmpty();
        RuleFor(x => x.Message).NotEmpty();
    }
}