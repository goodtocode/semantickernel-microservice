using System.Runtime.CompilerServices;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Services;

public class TextGenerationService(IChatCompletionService chatCompletionService) : ITextGenerationService
{
    private readonly IChatCompletionService _chatCompletionService = chatCompletionService;

    public IReadOnlyDictionary<string, object?> Attributes => throw new NotImplementedException();

    public async IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(
        string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var chatMessage in _chatCompletionService.GetStreamingChatMessageContentsAsync(prompt, executionSettings, kernel, cancellationToken))
        {
            yield return new StreamingTextContent(chatMessage.Content);
        }
    }
    public async Task<IReadOnlyList<TextContent>> GetTextContentsAsync(
        string prompt,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var chatMessageContents = await _chatCompletionService.GetChatMessageContentsAsync(prompt, executionSettings, kernel, cancellationToken);
        var textContents = chatMessageContents
            .Select(chatMessage => new TextContent(chatMessage.Content))
            .ToList();

        return textContents;
    }

}