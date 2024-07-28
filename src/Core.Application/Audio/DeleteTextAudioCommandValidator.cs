namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class DeleteTextAudioCommandValidator : AbstractValidator<DeleteTextAudioCommand>
{
    public DeleteTextAudioCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}