using FluentValidation;

namespace $safeprojectname$
{
    public class BusinessGetValidator : AbstractValidator<BusinessGetQuery>
    {
        public BusinessGetValidator()
        {
            RuleFor(v => v.BusinessKey).NotEmpty().NotNull();
        }
    }
}