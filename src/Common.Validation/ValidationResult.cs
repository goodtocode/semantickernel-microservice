namespace Goodtocode.Validation;

public class ValidationResult
{
    public ValidationResult() : this([])
    {
    }

    public ValidationResult(IEnumerable<ValidationFailure>? failures)
    {
        Errors = [.. (failures ?? [])];
    }


    public IReadOnlyList<ValidationFailure> Errors { get; }

    public bool IsValid => Errors.Count == 0;
}
