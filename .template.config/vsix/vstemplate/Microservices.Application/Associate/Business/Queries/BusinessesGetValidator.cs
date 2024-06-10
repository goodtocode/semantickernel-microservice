using FluentValidation;

namespace $safeprojectname$
{
    public class BusinessesGetValidator : AbstractValidator<BusinessesGetQuery>
    {
        public BusinessesGetValidator()
        {
            RuleFor(v => v.QueryPredicate).NotEmpty().NotNull();
        }
    }
}