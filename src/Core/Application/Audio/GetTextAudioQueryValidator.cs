namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class GetTextAudioQueryValidator : AbstractValidator<GetTextAudioQuery>
{
    public GetTextAudioQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}