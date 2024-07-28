namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class GetTextImageQueryValidator : AbstractValidator<GetTextImageQuery>
{
    public GetTextImageQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}