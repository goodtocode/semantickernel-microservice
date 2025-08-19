namespace Goodtocode.Validation;

public class ValidationFailure(string propertyName, string errorMessage)
{
    public string PropertyName { get; init; } = propertyName;
    public string ErrorMessage { get; init; } = errorMessage;
}
