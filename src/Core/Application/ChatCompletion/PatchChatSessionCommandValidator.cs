namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class PatchChatSessionCommandValidator : AbstractValidator<PatchChatSessionCommand>
{
    public PatchChatSessionCommandValidator()
    {
        RuleFor(x => x.Key).NotEmpty();
    }
}