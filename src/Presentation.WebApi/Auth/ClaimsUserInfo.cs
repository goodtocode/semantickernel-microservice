namespace Goodtocode.SemanticKernel.Presentation.WebApi.Auth;

/// <summary>
/// User information implementation that retrieves data from the current HTTP context.
/// </summary>
/// <param name="contextAccessor">HttpContext containing claims</param>
public class ClaimsUserInfo(IHttpContextAccessor contextAccessor) : IClaimsUserInfo
{
    private readonly HttpContext? context = contextAccessor?.HttpContext;

    /// <summary>
    /// Gets the unique identifier of the user object associated with the current context.
    /// </summary>
    /// <remarks>This property retrieves the value of the "objectidentifier" claim from the current user's context. 
    /// Ensure that the claim is present and properly formatted as a GUID in the authentication token.</remarks>
    public Guid ObjectId => Guid.TryParse(
        context?.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value,
        out var objectId) ? objectId : Guid.Empty;

    /// <summary>
    /// Gets the unique identifier of the tenant associated with the current user.
    /// </summary>
    /// <remarks>The tenant ID is extracted from the user's claims using the claim type 
    /// "http://schemas.microsoft.com/identity/claims/tenantid". Ensure that the claim is present and valid  in the
    /// user's identity for this property to return a meaningful value.</remarks>
    public Guid TenantId => Guid.TryParse(
        context?.User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value,
        out var tenantId) ? tenantId : Guid.Empty;

    /// <summary>
    /// Gets the first name of the user based on the associated claims.
    /// </summary>
    public string Givenname => context?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value ?? string.Empty;

    /// <summary>
    /// Gets the last name of the current user based on their claims.
    /// </summary>
    public string Surname => context?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value ?? string.Empty;

    /// <summary>
    /// Gets the email address of the current user based on their User Principal Name (UPN) claim.
    /// </summary>
    public string Email => context?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value ?? string.Empty;

    /// <summary>
    /// Gets the collection of scopes associated with the current user.
    /// </summary>
    /// <remarks>Scopes are typically used to define the permissions or access levels granted to the user. 
    /// This property retrieves the scopes from the user's claims, specifically from the claim with the type 
    /// "http://schemas.microsoft.com/identity/claims/scope".</remarks>
    public ICollection<string> Scopes => context?.User.FindFirst("http://schemas.microsoft.com/identity/claims/scope")?.Value?.Split(' ').ToList() ?? [];

    /// <summary>
    /// Gets the collection of roles associated with the current user.
    /// </summary>
    public ICollection<string> Roles => context?.User.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(c => c.Value).ToList() ?? [];

    /// <summary>
    /// Gets the collection of group names associated with the current user.
    /// </summary>
    public ICollection<string> Groups => context?.User.FindAll("groups").Select(c => c.Value).ToList() ?? [];
}