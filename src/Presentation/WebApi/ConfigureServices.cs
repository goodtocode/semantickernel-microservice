using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi;

/// <summary>
/// Presentation Layer WebApi Configuration
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Add WebUI Services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddWebUIServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllersWithViews(setupAction =>
            {
                setupAction.Filters.Add(
                    new ProducesDefaultResponseTypeAttribute());

                // ToDo: Setup Authentication with Bearer Token
                // setupAction.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
                setupAction.Filters.Add<ApiExceptionFilterAttribute>();
            })
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        services.AddEndpointsApiExplorer();

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin",
                options =>
                    options
                        .WithOrigins("https://localhost:7000")
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });

        services.AddSwaggerGen(setupAction =>
        {
            // ToDo: Setup Authentication with Bearer Token
            //setupAction.AddSecurityDefinition("Bearer",
            //    new OpenApiSecurityScheme
            //    {
            //        Description = "JWT Authorization header using the Bearer scheme.",
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "bearer"
            //    });

            //setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference {Id = "Bearer", Type = ReferenceType.SecurityScheme}
            //        },
            //        new List<string>()
            //    }
            //});

            setupAction.MapType<decimal>(() =>
                new OpenApiSchema
                {
                    Type = "number",
                    Format = "decimal"
                });
        });
        services.ConfigureOptions<ConfigureSwaggerOptions>();


        return services;
    }

    /// <summary>
    /// Swagger UI Configuration
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider"></param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// OpenApi Configuration
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }

        /// <summary>
        /// OpenApi Configuration
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = $"SemanticKernelMicroservice Service ({Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")})",
                Version = description.ApiVersion.ToString(),
                Description = $"An API to interact with the SemanticKernelMicroservice",
                Contact = new OpenApiContact
                {
                    //Email = "",
                    //Name = ""
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            };

            if (description.IsDeprecated) info.Description += " This API version has been deprecated.";

            return info;
        }
    }
}