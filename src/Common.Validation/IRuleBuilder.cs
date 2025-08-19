namespace Goodtocode.Validation;

public interface IRuleBuilder<T, TProperty>
{
    IRuleBuilder<T, TProperty> NotEmpty(string? errorMessage = null);
    IRuleBuilder<T, TProperty> LessThanOrEqualTo(Func<T, TProperty> other, string? errorMessage = null);
    IRuleBuilder<T, TProperty> GreaterThanOrEqualTo(Func<T, TProperty> other, string? errorMessage = null);
    IRuleBuilder<T, TProperty> When(Func<T, bool> predicate);
}
