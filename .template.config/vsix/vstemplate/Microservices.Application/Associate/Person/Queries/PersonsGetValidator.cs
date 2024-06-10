using FluentValidation;

namespace $safeprojectname$
{
    public class PersonsGetValidator : AbstractValidator<PersonsGetQuery>
    {
        public PersonsGetValidator()
        {
            RuleFor(v => v.QueryPredicate).NotEmpty().NotNull();
        }
    }
}