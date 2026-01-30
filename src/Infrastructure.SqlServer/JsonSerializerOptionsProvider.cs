using System.Text.Json;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer;

public static class JsonSerializerOptionsProvider
{
    public static readonly JsonSerializerOptions Default = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}