using Cannery.Aspects.Components.Auth;
using Cannery.Aspects.Components.Auth.Services;
using Goodtocode.SemanticKernel.Presentation.Blazor.Options;
using Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Services;
using Goodtocode.SemanticKernel.Presentation.Blazor.Services;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;
using Microsoft.Extensions.Options;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Reflection;

namespace Goodtocode.SemanticKernel.Presentation.Blazor;

public static class ConfigureServices
{

    public static bool IsLocal(this IWebHostEnvironment environment)
    {
        return environment.EnvironmentName.Equals("Local", StringComparison.OrdinalIgnoreCase);
    }

    public static void AddLocalEnvironment(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsLocal())
        {
            builder.Configuration
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddEnvironmentVariables();
            builder.WebHost.UseStaticWebAssets();
        }
    }

    public static void AddFrontendServices(this IServiceCollection services)
    {
        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<ILocalStorageService, LocalStorageService>();
        services.AddScoped<IChatService, ChatService>();
    }

    public static IServiceCollection AddUserClaimsSyncService(this IServiceCollection services)
    {
        services.AddScoped<IUserClaimsInfo, UserClaimsInfo>();
        services.AddScoped<IUserSyncService, UserSyncService>();

        return services;
    }

    public static IServiceCollection AddBackendApi(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<BackendApiOptions>()
        .Bind(configuration.GetSection(BackendApiOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddAccessTokenHttpClient(options =>
        {
            options.BaseAddress = new Uri(configuration["BackendApi:BaseUrl"] ?? throw new InvalidOperationException("Base URL for BackEndApi is not configured."));
            options.ClientName = "BackendApiClient";
            options.MaxRetry = 3;
        });

        services.AddScoped<BackendApiClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<BackendApiOptions>>().Value;
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("BackendApiClient");
            return new BackendApiClient(options.BaseUrl.ToString(), httpClient);
        });

        return services;
    }
}