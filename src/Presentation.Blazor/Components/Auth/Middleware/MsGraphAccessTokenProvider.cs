using Goodtocode.SecuredHttpClient.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Components.Auth.Middleware;

public class MsGraphAccessTokenProvider(IHttpContextAccessor httpContextAccessor) : IAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<string> GetAccessTokenAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            return string.Empty;

        var accessToken = await context.GetTokenAsync(OpenIdConnectDefaults.AuthenticationScheme, "access_token");
        return accessToken ?? string.Empty;
    }
}