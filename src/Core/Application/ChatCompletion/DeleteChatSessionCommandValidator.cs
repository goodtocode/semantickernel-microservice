namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class DeleteChatSessionCommandValidator : AbstractValidator<DeleteChatSessionCommand>
{
    public DeleteChatSessionCommandValidator()
    {
        RuleFor(x => x.Key).NotEmpty();
    }
}