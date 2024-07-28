namespace Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;

public class CustomNotFoundException : Exception
{
    public CustomNotFoundException()
    {
    }

    public CustomNotFoundException(string message)
        : base(message)
    {
    }

    public CustomNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CustomNotFoundException(string name, object id)
        : base($"Entity \"{name}\" ({id}) was not found.")
    {
    }
}