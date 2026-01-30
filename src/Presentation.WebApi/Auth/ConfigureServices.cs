
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.Auth;

/// <summary>
/// Presentation Layer WebApi Configuration
/// </summary>
public static class ConfigureServices
{
    private struct TokenRoleClaimTypes
    {
        public const string Roles = "roles";
        public const string Groups = "groups"; // I.e. EID Security Groups
    }

    /// <summary>
    /// AddUserInfo
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthenticationWithRoles(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(
                    jwtOptions =>
                    {
                        configuration.GetSection("EntraExternalId").Bind(jwtOptions);
                        //jwtOptions.TokenValidationParameters.RoleClaimType = TokenRoleClaimTypes.Roles;
                    },
                    identityOptions =>
                    {
                        configuration.GetSection("EntraExternalId").Bind(identityOptions);
                    });

        services.AddScoped<IClaimsUserInfo, ClaimsUserInfo>();
        services.AddScoped(typeof(IPipelineBehavior<>), typeof(UserInfoBehavior<>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UserInfoBehavior<,>));

        return services;
    }
}