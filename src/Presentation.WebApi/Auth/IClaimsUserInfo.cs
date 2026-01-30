namespace Goodtocode.SemanticKernel.Presentation.WebApi.Auth;

/// <summary>
/// Represents a user's information, including identifiers, personal details, and contact information.
/// </summary>
/// <remarks>This interface provides a standardized structure for accessing user-related data, such as unique
/// identifiers, name details, and email address. It is commonly used in scenarios where user identity and contact
/// information need to be retrieved or processed.</remarks>
public interface IClaimsUserInfo
{
    /// <summary>
    /// Gets the unique identifier for the object.
    /// </summary>
    Guid ObjectId { get; }

    /// <summary>
    /// Gets the unique identifier of the tenant associated with the current context.
    /// </summary>
    Guid TenantId { get; }

    /// <summary>
    /// Gets the first name of the individual.
    /// </summary>
    string Givenname { get; }

    /// <summary>
    /// Gets the last name of the individual.
    /// </summary>
    string Surname { get; }

    /// <summary>
    /// Gets the email address associated with the entity.
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Gets the highest role
    /// </summary>
    ICollection<string>  Roles { get; }

    /// <summary>
    /// Gets the collection of scopes associated with the current operation.
    /// </summary>
    ICollection<string> Scopes { get; }

    /// <summary>
    /// Gets the collection of group names associated with the current user.
    /// </summary>
    ICollection<string> Groups { get; }
}
