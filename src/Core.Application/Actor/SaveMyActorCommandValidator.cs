namespace Goodtocode.SemanticKernel.Core.Application.Actor;

public class SaveMyActorCommandValidator : Validator<SaveMyActorCommand>
{
    public SaveMyActorCommandValidator()
    {
        RuleFor(x => x.UserInfo).NotEmpty();
        RuleFor(x => x.TenantId).NotEmpty();
    }
}