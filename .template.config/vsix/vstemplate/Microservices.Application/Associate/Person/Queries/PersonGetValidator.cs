using FluentValidation;

namespace $safeprojectname$
{
    public class PersonGetValidator : AbstractValidator<PersonGetQuery>
    {
        public PersonGetValidator()
        {
            RuleFor(v => v.PersonKey).NotEmpty().NotNull();
        }
    }
}