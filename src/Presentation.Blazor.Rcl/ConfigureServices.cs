using Goodtocode.Presentation.WebApi.Client;
using Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Presentation.Blazor.Rcl;

public static class ConfigureServices
{
    public static IServiceCollection AddPresentationWebApiServices(this IServiceCollection services,
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
