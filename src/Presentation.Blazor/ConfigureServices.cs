using Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Options;
using Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Services;
using Goodtocode.SemanticKernel.Presentation.Blazor.Services;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;
using Microsoft.Extensions.Options;
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
        if (builder.Environment.IsEnvironment("Local"))
        {
            builder.Configuration
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddEnvironmentVariables();
            builder.WebHost.UseStaticWebAssets();
        }
    }

    public static void AddBlazorServices(this IServiceCollection services)
    {
        services.AddScoped<ILocalStorageService, LocalStorageService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChatService, ChatService>();
    }

    public static IServiceCollection AddBackEndApi(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<WebApiOptions>()
        .Bind(configuration.GetSection(WebApiOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped(builder =>
        {
            var options = builder.GetRequiredService<IOptions<WebApiOptions>>().Value;
            return new WebApiClient(options.BaseUrl.ToString(), new HttpClient());
        });

        return services;
    }
}
