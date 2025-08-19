namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class CreateChatSessionCommandValidator : Validator<CreateChatSessionCommand>
{
    public CreateChatSessionCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty();
    }
}