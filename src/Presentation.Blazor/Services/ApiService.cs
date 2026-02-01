using Goodtocode.SemanticKernel.Presentation.WebApi.Client;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;

public abstract class ApiService
{
    protected static async Task HandleApiException(Func<Task> apiCall)
    {
        try
        {
            await apiCall().ConfigureAwait(false);
        }
        catch (ApiException ex) when (ex.StatusCode == 400)
        {
            var errors = ParseValidationErrors(ex.Response);
            throw new ValidationException("Validation failed", null, errors);
        }
    }

    protected static async Task<T> HandleApiException<T>(Func<Task<T>> apiCall)
    {
        try
        {
            return await apiCall().ConfigureAwait(false);
        }
        catch (ApiException ex) when (ex.StatusCode == 400)
        {
            var errors = ParseValidationErrors(ex.Response);
            throw new ValidationException("Validation failed", null, errors);
        }
    }

    protected static Dictionary<string, List<string>> ParseValidationErrors(string content)
    {
        var result = new Dictionary<string, List<string>>();
        if (!string.IsNullOrEmpty(content))
        {
            var doc = JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty("errors", out var errorsElement))
            {
                foreach (var property in errorsElement.EnumerateObject())
                {
                    result[property.Name] = property.Value
                        .EnumerateArray()
                        .Select(e => e.GetString())
                        .Where(s => s != null)
                        .ToList()!;
                }
            }
        }
        return result;
    }
}
