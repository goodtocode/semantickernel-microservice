namespace Goodtocode.Validation;

public class ValidationRule<T>
{
    public Func<T, bool> Condition { get; set; } = _ => true;
    public required Func<T, string> ErrorMessage { get; set; }
    public required Func<T, bool> IsValid { get; set; }
}
