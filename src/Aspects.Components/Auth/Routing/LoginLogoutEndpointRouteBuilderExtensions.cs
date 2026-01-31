using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Goodtocode.Aspects.Components.Auth.Routing;

public static class LoginLogoutEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapSignInSignOut(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(string.Empty);

        group.MapGet("/SignIn", (string? returnUrl) => TypedResults.Challenge(GetAuthProperties(returnUrl)))
            .AllowAnonymous();

        group.MapGet("/SignOut", (string? returnUrl) => TypedResults.SignOut(GetAuthProperties(returnUrl),
            [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]));

        return group;
    }

    private static AuthenticationProperties GetAuthProperties(string? returnUrl) =>
        new()
        {
            RedirectUri = returnUrl switch
            {
                string => new Uri(returnUrl, UriKind.Absolute).PathAndQuery,
                null => "/",
            }
        };
}