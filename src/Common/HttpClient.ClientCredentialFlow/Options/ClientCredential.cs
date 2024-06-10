namespace GoodToCode.HttpClient.ClientCredentialFlow.Options;

public class ClientCredential
{
    public ClientCredential(string clientId, string clientSecret, string tokenUrl, string scope)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        TokenUrl = tokenUrl;
        Scope = scope;
    }

    public string ClientId { get; }
    public string ClientSecret { get; }
    public string TokenUrl { get; }
    public string Scope { get; }
}
