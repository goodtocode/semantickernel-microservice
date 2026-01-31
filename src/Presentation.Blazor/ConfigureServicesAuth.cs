using Goodtocode.SemanticKernel.Presentation.Blazor.Components.Auth.Middleware;
using Goodtocode.SemanticKernel.Presentation.Blazor.Components.Auth.Services;
using Goodtocode.SemanticKernel.Presentation.Blazor.Options;
using Goodtocode.SecuredHttpClient.Middleware;
using Goodtocode.SecuredHttpClient.Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

namespace Goodtocode.SemanticKernel.Presentation.Blazor;

public static class ConfigureServicesAuth
{
    private struct TokenRoleClaimTypes
    {
        public const string Roles = "roles";
        public const string Groups = "groups";
    }

    public static void AddAuthenticationForDownstream(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(options =>
                {
                    configuration.GetSection("EntraExternalId").Bind(options);
                    options.SignInScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches()
            .AddDownstreamApi("BackendApi", configOptions =>
            {
                configOptions.BaseUrl = configuration["BackendApi:BaseUrl"];
                configOptions.Scopes = [$"api://{configuration["BackendApi:ClientId"] ?? Guid.Empty.ToString()}/.default",
                "User.Read"];
            });

        services.Configure<MicrosoftIdentityOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.SaveTokens = true;
            options.Events = new OpenIdConnectEvents
            {
                OnTokenValidated = context =>
                {
                    var syncService = context.HttpContext.RequestServices.GetService<IUserSyncService>();
                    syncService?.UserChanged(context.Principal);
                    return Task.CompletedTask;
                }
            };
        });

        ////For development: .AddInMemoryTokenCaches()
        ////For Production: .AddDistributedTokenCaches()
        ////services.AddDistributedMemoryCache();
        //services.Configure<MsalDistributedTokenCacheAdapterOptions>(
        //    options =>
        //    {
        //        // Disable L1 Cache default: false
        //        //options.DisableL1Cache = false;

        //        // L1 Cache Size Limit default: 500 MB
        //        //options.L1CacheOptions.SizeLimit = 500 * 1024 * 1024;

        //        // Encrypt tokens at rest default: false
        //        options.Encrypt = true;

        //        // Sliding Expiration default: 1 hour
        //        //options.SlidingExpiration = TimeSpan.FromHours(1);
        //    });

        ///* When you move to web farm testing with tokens encrypted at rest while
        // * hosting multiple app instances and stop using AddDistributedMemoryCache 
        // * in favor of a production distributed token cache provider, enable the 
        // * following code, which configures Data Protection to protect keys with 
        // * Azure Key Vault and maintain keys in Azure Blob Storage.
        // * Our recommended approach for using Azure Blob Storage and Azure Key 
        // * Vault is to use an Azure Managed Identity in production.
        // * Give the Managed Identity 'Key Vault Crypto User' and 
        // * 'Storage Blob Data Contributor' roles. Assign the Managed Identity
        // * to the App Service in Settings > Identity > User assigned > Add.
        // * Other options, both within Azure and outside of Azure, are available for
        // * managing Data Protection keys across multiple app instances. See the 
        // * ASP.NET Core Data Protection documentation for details.

        //// Requires the Microsoft.Extensions.Azure NuGet package
        //services.TryAddSingleton<AzureEventSourceLogForwarder>();

        //TokenCredential? credential;

        //if (builder.Environment.IsProduction())
        //{
        //    credential = new ManagedIdentityCredential("{MANAGED IDENTITY CLIENT ID}");
        //}
        //else
        //{
        //    // Local development and testing only
        //    DefaultAzureCredentialOptions options = new()
        //    {
        //        // Specify the tenant ID to use the dev credentials when running the app locally
        //        // in Visual Studio.
        //        VisualStudioTenantId = "{TENANT ID}",
        //        SharedTokenCacheTenantId = "{TENANT ID}"
        //    };

        //    credential = new DefaultAzureCredential(options);
        //}

        //services.AddDataProtection()
        //    .SetApplicationName("BlazorWebAppEntra")
        //    .PersistKeysToAzureBlobStorage(new Uri("{BLOB URI}"), credential)
        //    .ProtectKeysWithAzureKeyVault( new Uri("{KEY IDENTIFIER}"), credential);
        //*/
        //// Add DI
        ////services.AddScoped<TokenAcquisition>();
        //// Use
        ////await tokenAcquisition.GetAccessTokenForUserAsync(new[] { "your-scope" }, OpenIdConnectDefaults.AuthenticationScheme);
    }

    public static IServiceCollection AddAccessTokenHttpClient(
        this IServiceCollection services,
        Action<ResilientHttpClientOptions> configureOptions)
    {
        var options = new ResilientHttpClientOptions();
        configureOptions(options);

        if (options.BaseAddress == null)
            throw new ArgumentNullException(nameof(configureOptions), "BaseAddress must be provided.");
        if (string.IsNullOrWhiteSpace(options.ClientName))
            throw new ArgumentNullException(nameof(configureOptions), "ClientName must be provided.");

        services.AddOptions<AuthCodePkceOptions>()
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddScoped<IAccessTokenProvider, DownstreamApiAccessTokenProvider>();
        services.AddScoped<TokenHandler>();

        services.AddHttpClient(options.ClientName, clientOptions =>
        {
            clientOptions.DefaultRequestHeaders.Clear();
            clientOptions.BaseAddress = options.BaseAddress;
        })
        .AddHttpMessageHandler<TokenHandler>()
        .AddStandardResilienceHandler(resilienceOptions =>
        {
            resilienceOptions.Retry.UseJitter = true;
            resilienceOptions.Retry.MaxRetryAttempts = options.MaxRetry;
        });

        return services;
    }
}
