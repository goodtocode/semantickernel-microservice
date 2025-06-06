using Goodtocode.HttpClient.ClientCredentialFlow.Middleware;
using Goodtocode.HttpClient.ClientCredentialFlow.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goodtocode.HttpClient.ClientCredentialFlow;

public static class ConfigureServices
{
    public static IServiceCollection AddHttpClientJitterService(this IServiceCollection services,
        IConfiguration configuration,
        string clientName,
        Uri baseAddress,
        int maxRetry = 5)
    {
        // Add strongly-typed and validated options for downstream use via DI.
        services.AddOptions<ClientCredential>()
        .Bind(configuration.GetSection(nameof(ClientCredential)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton<BearerToken>();
        services.AddTransient<TokenHandler>();

        services.AddHttpClient(clientName, options =>
        {
            options.DefaultRequestHeaders.Clear();
            options.BaseAddress = baseAddress;
        })
            .AddHttpMessageHandler<TokenHandler>()
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.UseJitter = true;
                options.Retry.MaxRetryAttempts = maxRetry;
            });

        return services;
    }
}