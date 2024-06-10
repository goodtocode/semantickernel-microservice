using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace $safeprojectname$
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, string connectionStringKey)
        {
            services.AddDbContextPool<AssociateDbContext>(options =>
            {
                options.UseSqlServer(configuration[connectionStringKey]);
            });

            services.AddScoped<IAssociateDbContext, AssociateDbContext>();

            return services;
        }
    }
}