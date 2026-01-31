using Goodtocode.SecuredHttpClient.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

namespace Goodtocode.Aspects.Components.Auth.Middleware;

public class DownstreamApiAccessTokenProvider(IHttpContextAccessor httpContextAccessor, ITokenAcquisition tokenAcquisition, IConfiguration configuration) : IAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ITokenAcquisition _tokenAcquisition = tokenAcquisition;
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> GetAccessTokenAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (!context?.User?.Identity?.IsAuthenticated == true)
            return string.Empty;

        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync([
            $"api://{_configuration["BackendApi:ClientId"] ?? Guid.Empty.ToString()}/.default"
        ], user: context?.User);

        return accessToken ?? string.Empty;
    }
}