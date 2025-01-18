using Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Options;
using Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.AudioToText;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.TextGeneration;
using Microsoft.SemanticKernel.TextToAudio;
using Microsoft.SemanticKernel.TextToImage;

namespace Goodtocode.SemanticKernel.Infrastructure.SemanticKernel;

public static class ConfigureServices
{
    public static IServiceCollection AddSemanticKernelMemoryServices(this IServiceCollection services)
    {
        //var memory = new KernelMemoryBuilder()
        //    .WithOpenAIDefaults(Env.Var("OPENAI_API_KEY"))
        //    .WithSqlServerMemoryDb("YourSqlConnectionString")
        //    .Build<MemoryServerless>();

        return services;
    }

    public static IServiceCollection AddSemanticKernelServices(this IServiceCollection services,
    IConfiguration configuration)
    {
        // Add strongly-typed and validated options for downstream use via DI.
        services.AddOptions<OpenAIOptions>()
        .Bind(configuration.GetSection(nameof(OpenAI)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddKernel();

        // Chat Completion
        services.AddSingleton<IChatCompletionService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
            return new OpenAIChatCompletionService(modelId: options.ChatCompletionModelId, apiKey: options.ApiKey);
        });
        
        // TextGenerationService deprecated. Use custom connector service instead.
        services.AddSingleton<ITextGenerationService, TextGenerationService>();

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        // Translate audio to text
        services.AddSingleton<IAudioToTextService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
            return new OpenAIAudioToTextService(modelId: options.AudioModelId, apiKey: options.ApiKey);
        })
        // Translate audio to text
        .AddSingleton<ITextToAudioService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
            return new OpenAITextToAudioService(modelId: options.AudioModelId, apiKey: options.ApiKey);
        })
        // Embedding text into a vector for storage in CosmosDb or Qdrant
        .AddSingleton<ITextEmbeddingGenerationService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
            return new OpenAITextEmbeddingGenerationService(modelId: options.TextEmbeddingModelId, apiKey: options.ApiKey);
        })
        // Translate text to image
        .AddSingleton<ITextToImageService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenAIOptions>>().Value;
            return new OpenAITextToImageService(modelId: options.ImageModelId, apiKey: options.ApiKey);
        });
#pragma warning restore SKEXP0001
#pragma warning restore SKEXP0010

        // ToDo: Implement MemoryBuilder.WithMemoryStore(VolatileMemoryStore or SQLMemoryStore)

        return services;
    }
}