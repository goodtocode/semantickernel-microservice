namespace Goodtocode.Assertion;

public class AssertionScope : IDisposable
{
    private static readonly AsyncLocal<Stack<List<string>>> _scopes = new();

    public AssertionScope()
    {
        if (_scopes.Value == null)
            _scopes.Value = new Stack<List<string>>();
        _scopes.Value.Push([]);
    }

    public static void AddFailure(string message)
    {
        if (_scopes.Value != null && _scopes.Value.Count > 0)
            _scopes.Value.Peek().Add(message);
    }

    public void Dispose()
    {
        var failures = _scopes.Value?.Pop();
        if (failures != null && failures.Count > 0)
        {
            throw new AssertionFailedException(string.Join(Environment.NewLine, failures));
        }
        GC.SuppressFinalize(this);
    }

    public static bool IsActive => _scopes.Value != null && _scopes.Value.Count > 0;
}
