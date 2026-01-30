namespace Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;

public class CustomForbiddenAccessException : Exception
{
    public CustomForbiddenAccessException()
    {
    }

    public CustomForbiddenAccessException(string message)
        : base(message)
    {
    }

    public CustomForbiddenAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CustomForbiddenAccessException(string name, object id)
        : base($"Entity \"{name}\" ({id}) was not found.")
    {
    }
}