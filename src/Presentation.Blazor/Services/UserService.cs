namespace Goodtocode.SemanticKernel.Presentation.Blazor.Services;

public interface IUserService
{
    Task<Guid> GetUserIdAsync();
}

public class UserService(ILocalStorageService storageService) : IUserService
{
    private readonly ILocalStorageService _storageService = storageService;

    public const string UserIdKey = $"userId";

    public async Task<Guid> GetUserIdAsync()
    {
        var storedUserId = await _storageService.GetItemAsync(UserIdKey);
        if (!Guid.TryParse(storedUserId, out Guid userId))
        {
            userId = Guid.NewGuid();
            await _storageService.SetItemAsync(UserIdKey, userId.ToString());
        }

        return userId;
    }
}