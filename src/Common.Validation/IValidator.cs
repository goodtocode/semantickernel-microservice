namespace Goodtocode.Validation;

public interface IValidator<T>
{
    void ValidateAndThrow(T instance);
}