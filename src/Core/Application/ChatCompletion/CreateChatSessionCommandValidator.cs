namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class AddChatSessionCommandValidator : AbstractValidator<CreateChatSessionCommand>
{
    public AddChatSessionCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty();
    }
}