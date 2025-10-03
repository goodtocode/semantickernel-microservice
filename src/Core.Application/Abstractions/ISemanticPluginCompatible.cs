namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

public interface ISemanticPluginCompatible
{
    string PluginName { get; }
    string FunctionName { get; }
    Dictionary<string, object> Parameters { get; }

}