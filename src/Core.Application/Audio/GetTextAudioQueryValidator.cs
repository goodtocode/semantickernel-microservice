namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class GetTextAudioQueryValidator : Validator<GetTextAudioQuery>
{
    public GetTextAudioQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}