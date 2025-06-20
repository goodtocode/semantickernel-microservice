﻿using Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Options;
using Goodtocode.SemanticKernel.Presentation.WebApi.Client;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Goodtocode.SemanticKernel.Presentation.Blazor;

public static class ConfigureServices
{

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
