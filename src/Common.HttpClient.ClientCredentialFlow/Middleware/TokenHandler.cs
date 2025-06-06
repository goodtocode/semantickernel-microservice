using System.Net.Http.Headers;

namespace Goodtocode.HttpClient.ClientCredentialFlow.Middleware;

public class TokenHandler : DelegatingHandler
{
    private readonly BearerToken _accessToken;

    public TokenHandler(BearerToken bearerToken)
    {
        _accessToken = bearerToken;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _accessToken.GetAccessTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}