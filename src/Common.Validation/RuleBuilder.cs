using System.Globalization;
using System.Linq.Expressions;

namespace Goodtocode.Validation;

public class RuleBuilder<T, TProp>
{
    private readonly Expression<Func<T, TProp>> _selector;
    private readonly string _propertyName;
    private readonly List<Func<T, ValidationFailure?>> _rules;
    private Func<T, bool>? _condition;

    public RuleBuilder(Expression<Func<T, TProp>> selector, string propertyName, List<Func<T, ValidationFailure?>> rules)
    {
        _selector = selector;
        _propertyName = propertyName;
        _rules = rules;
    }

    public RuleBuilder<T, TProp> NotEmpty(string? errorMessage = null)
    {
        _rules.Add(instance =>
        {
            if (_condition != null && !_condition(instance)) return null;

            var value = _selector.Compile()(instance);

            bool isValid = value switch
            {
                null => false,
                Guid guid => guid != Guid.Empty,
                string str => !string.IsNullOrWhiteSpace(str),
                int i => i != 0,
                long l => l != 0L,
                double d => d != 0.0,
                decimal m => m != 0m,
                DateTime dt => dt != DateTime.MinValue,
                Enum e => Convert.ToInt32(e, CultureInfo.InvariantCulture) != 0,
                _ => true
            };

            return isValid ? null : new ValidationFailure(_propertyName, errorMessage ?? $"{_propertyName} must not be empty");
        });
        return this;
    }

    public RuleBuilder<T, TProp> NotEqual(TProp other, string? errorMessage = null)
    {
        _rules.Add(instance =>
        {
            if (_condition != null && !_condition(instance)) return null;

            var value = _selector.Compile()(instance);
            var comparison = Comparer<TProp>.Default.Compare(value, other);
            return comparison != 0 ? null : new ValidationFailure(_propertyName, errorMessage ?? $"{_propertyName} must not be equal to {other}");
        });
        return this;
    }

    public RuleBuilder<T, TProp> Equal(TProp other, string? errorMessage = null)
    {
        _rules.Add(instance =>
        {
            if (_condition != null && !_condition(instance)) return null;

            var value = _selector.Compile()(instance);
            var comparison = Comparer<TProp>.Default.Compare(value, other);
            return comparison == 0 ? null : new ValidationFailure(_propertyName, errorMessage ?? $"{_propertyName} must not be equal to {other}");
        });
        return this;
    }

    public RuleBuilder<T, TProp> LessThanOrEqualTo(Func<T, TProp> otherSelector, string? errorMessage = null)
    {
        _rules.Add(instance =>
        {
            if (_condition != null && !_condition(instance)) return null;

            var value = _selector.Compile()(instance);
            var other = otherSelector(instance);
            var comparison = Comparer<TProp>.Default.Compare(value, other);
            return comparison <= 0 ? null : new ValidationFailure(_propertyName, errorMessage ?? $"{_propertyName} must be ≤ {other}");
        });
        return this;
    }

    public RuleBuilder<T, TProp> GreaterThanOrEqualTo(Func<T, TProp> otherSelector, string? errorMessage = null)
    {
        _rules.Add(instance =>
        {
            if (_condition != null && !_condition(instance)) return null;

            var value = _selector.Compile()(instance);
            var other = otherSelector(instance);
            var comparison = Comparer<TProp>.Default.Compare(value, other);
            return comparison >= 0 ? null : new ValidationFailure(_propertyName, errorMessage ?? $"{_propertyName} must be ≥ {other}");
        });
        return this;
    }

    public RuleBuilder<T, TProp> When(Func<T, bool> condition)
    {
        _condition = condition;
        return this;
    }
}
