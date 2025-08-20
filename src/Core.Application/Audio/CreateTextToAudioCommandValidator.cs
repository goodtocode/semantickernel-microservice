namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class CreateTextToAudioCommandValidator : Validator<CreateTextToAudioCommand>
{
    public CreateTextToAudioCommandValidator()
    {
        RuleFor(x => x.Prompt).NotEmpty();
    }
}