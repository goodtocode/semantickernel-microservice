namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;

public interface IUserService
{
    Task<Guid> GetUserIdAsync();
}

public class UserService(ILocalStorageService storageService, IHttpContextAccessor contextAccessor) : IUserService
{
    private readonly ILocalStorageService _storageService = storageService;
    private readonly HttpContext? context = contextAccessor?.HttpContext;

    public const string UserIdKey = "userId";

    public async Task<Guid> GetUserIdAsync()
    {
        return await GetUserIdFromCookieAsync();
    }

    private async Task<Guid> GetUserIdFromLocalStorageAsync()
    {
        var storedUserId = await _storageService.GetItemAsync(UserIdKey);
        if (!Guid.TryParse(storedUserId, out Guid userId))
        {
            userId = Guid.NewGuid();
            await _storageService.SetItemAsync(UserIdKey, userId.ToString());
        }

        return userId;
    }

    private async Task<Guid> GetUserIdFromCookieAsync()
    {
        var userIdCookie = context?.Request.Cookies[UserIdKey];
        var userId = Guid.NewGuid();
        await Task.Run(() =>
        {
            if (!string.IsNullOrEmpty(userIdCookie))
            {
                userId = Guid.Parse(userIdCookie);
            }
            else
            {
                context?.Response.Cookies.Append(UserIdKey, userId.ToString(), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
                userIdCookie = userId.ToString();
            }
        });


        return userId;
    }
}