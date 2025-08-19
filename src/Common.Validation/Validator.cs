using System.Linq.Expressions;

namespace Goodtocode.Validation;

public abstract class Validator<T> : IValidator<T>
{
    private readonly List<Func<T, ValidationFailure?>> _rules = [];

    protected RuleBuilder<T, TProp> RuleFor<TProp>(
        Expression<Func<T, TProp>> propertySelector)
    {
        var propertyName = GetPropertyName(propertySelector);
        return new RuleBuilder<T, TProp>(propertySelector, propertyName, _rules);
    }

    public ValidationResult Validate(T instance)
    {
        var failures = _rules
            .Select(rule => rule(instance))
            .Where(f => f != null)
            .Cast<ValidationFailure>()
            .ToList();

        return new ValidationResult(failures);
    }

    public async Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => Validate(instance), cancellationToken);
    }

    public void ValidateAndThrow(T instance)
    {
        var result = Validate(instance);
        if (!result.IsValid)
            throw new CustomValidationException(result.Errors!);
    }

    public async Task ValidateAndThrowAsync(T instance, CancellationToken cancellationToken = default)
    {
        var result = await ValidateAsync(instance, cancellationToken);
        if (!result.IsValid)
            throw new CustomValidationException(result.Errors!);
    }

    private static string GetPropertyName<TProp>(Expression<Func<T, TProp>> expression)
    {
        if (expression.Body is MemberExpression member)
            return member.Member.Name;
        throw new ArgumentException("Expression must be a property access.", nameof(expression));
    }
}