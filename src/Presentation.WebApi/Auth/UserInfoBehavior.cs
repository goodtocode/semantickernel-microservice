using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Domain.Auth;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.Auth;

/// <summary>
/// Represents a pipeline behavior that injects user information into a request  before passing it to the next
/// delegate in the pipeline.
/// </summary>
/// <remarks>This behavior ensures that the <see cref="IUserEntity"/> instance is assigned to the  <see
/// cref="IUserInfoRequest.UserInfo"/> property of the request before invoking the next delegate.</remarks>
/// <typeparam name="TRequest">The type of the request. Must implement <see cref="IUserInfoRequest"/>.</typeparam>
/// <param name="userInfo"></param>
public class UserInfoBehavior<TRequest>(IClaimsUserInfo userInfo) : IPipelineBehavior<TRequest>
       where TRequest : IUserInfoRequest
{
    /// <summary>
    /// Processes the specified request and invokes the next delegate in the request pipeline.
    /// </summary>
    /// <remarks>This method modifies the <paramref name="request"/> by setting its <c>UserInfo</c>
    /// property before invoking the next handler. Ensure that <paramref name="nextInvoker"/> is not null and is
    /// called to continue the request pipeline.</remarks>
    /// <param name="request">The request to be processed. The request object may be modified during processing.</param>
    /// <param name="nextInvoker">The delegate to invoke the next handler in the pipeline. This delegate must be called to continue processing
    /// the request.</param>
    /// <param name="cancellationToken">A token that can be used to propagate notification that the operation should be canceled.</param>    
    public async Task Handle(TRequest request, RequestDelegateInvoker nextInvoker, CancellationToken cancellationToken)
    {
        request.UserInfo = UserEntity.Create(userInfo.ObjectId, userInfo.TenantId, userInfo.Givenname, userInfo.Surname, userInfo.Email, userInfo.Roles);
        await nextInvoker();
    }
}

/// <summary>
/// Represents a pipeline behavior that injects user information into a request  before passing it to the next
/// delegate in the pipeline.
/// </summary>
/// <remarks>This behavior ensures that the <see cref="IUserEntity"/> instance is assigned to the  <see
/// cref="IUserInfoRequest.UserInfo"/> property of the request before invoking the next delegate.</remarks>
/// <typeparam name="TRequest">The type of the request. Must implement <see cref="IUserInfoRequest"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="userInfo"></param>
public class UserInfoBehavior<TRequest, TResponse>(IClaimsUserInfo userInfo) : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IUserInfoRequest
{
    /// <summary>
    /// Processes the specified request and invokes the next delegate in the request pipeline.
    /// </summary>
    /// <remarks>This method modifies the <paramref name="request"/> by setting its <c>UserInfo</c>
    /// property before invoking the next handler. Ensure that <paramref name="nextInvoker"/> is not null and is
    /// called to continue the request pipeline.</remarks>
    /// <param name="request">The request to be processed. The request object may be modified during processing.</param>
    /// <param name="nextInvoker">The delegate to invoke the next handler in the pipeline. This delegate must be called to continue processing
    /// the request.</param>
    /// <param name="cancellationToken">A token that can be used to propagate notification that the operation should be canceled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response of type <typeparamref name="TResponse"/>.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestDelegateInvoker<TResponse> nextInvoker, CancellationToken cancellationToken)
    {
        request.UserInfo = UserEntity.Create(userInfo.ObjectId, userInfo.TenantId, userInfo.Givenname, userInfo.Surname, userInfo.Email, userInfo.Roles);
        return await nextInvoker();
    }
}
