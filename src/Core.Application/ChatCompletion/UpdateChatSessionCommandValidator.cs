namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion;

public class UpdateChatSessionCommandValidator : Validator<UpdateChatSessionCommand>
{
    public UpdateChatSessionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}