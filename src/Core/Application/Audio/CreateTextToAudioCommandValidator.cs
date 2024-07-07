namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class CreateTextToAudioCommandValidator : AbstractValidator<CreateTextToAudioCommand>
{
    public CreateTextToAudioCommandValidator()
    {
        RuleFor(x => x.Prompt).NotEmpty();
    }
}