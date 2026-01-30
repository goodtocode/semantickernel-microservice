namespace Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;

public class CustomConflictException : Exception
{
    public CustomConflictException()
    {
    }

    public CustomConflictException(string message)
        : base(message)
    {
    }

    public CustomConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CustomConflictException(string name, object id)
        : base($"Entity \"{name}\" ({id}) conflicts with an existing entity.")
    {
    }
}