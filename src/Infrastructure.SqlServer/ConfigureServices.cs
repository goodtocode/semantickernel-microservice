using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer;

public static class ConfigureServices
{
    public static IServiceCollection AddDbContextServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<SemanticKernelContext>(options =>
                options.UseInMemoryDatabase("DefaultConnection").UseLazyLoadingProxies());
        }
        else
        {
            services.AddDbContext<SemanticKernelContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsAssembly(typeof(SemanticKernelContext).Assembly.FullName))
                    .UseLazyLoadingProxies());
        }

        services.AddScoped<ISemanticKernelContext, SemanticKernelContext>();
        services.AddScoped<SemanticKernelContextInitializer>();

        return services;
    }
}