using Goodtocode.SemanticKernel.Presentation.Blazor.Services;

namespace Goodtocode.SemanticKernel.Presentation.Blazor.Utilities;

public interface IUserUtility
{
    Task<Guid> GetUserIdAsync();
}

public class UserUtility(ILocalStorageService storageService) : IUserUtility
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