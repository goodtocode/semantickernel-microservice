using Goodtocode.SemanticKernel.Core.Application.Audio;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class CreateAuthorCommandValidator : Validator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}