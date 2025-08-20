namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class DeleteTextAudioCommandValidator : Validator<DeleteTextAudioCommand>
{
    public DeleteTextAudioCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}