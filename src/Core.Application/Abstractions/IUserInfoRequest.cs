using Goodtocode.SemanticKernel.Core.Domain.Auth;

namespace Goodtocode.SemanticKernel.Core.Application.Abstractions;

/// <summary>
/// Represents a request containing user information.
/// </summary>
/// <remarks>This interface is used to encapsulate user information in a request.  The <see
/// cref="UserInfo"/> property allows getting or setting the associated user details.</remarks>
public interface IUserInfoRequest
{
    /// <summary>
    /// Gets or sets the user information associated with the current context.
    /// </summary>
    IUserEntity? UserInfo { get; set; }
}
