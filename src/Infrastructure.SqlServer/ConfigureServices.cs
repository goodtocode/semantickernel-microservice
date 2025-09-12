using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer;

public static class ConfigureServices
{
    public static IServiceCollection AddDbContextServices(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddDbContext<SemanticKernelContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(SemanticKernelContext).Assembly.FullName))
                .UseLazyLoadingProxies());

        services.AddScoped<ISemanticKernelContext, SemanticKernelContext>();
        services.AddScoped<SemanticKernelContextInitializer>();

        return services;
    }
}