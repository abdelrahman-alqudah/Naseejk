using Naseejk.core.Common;
using Naseejk.core.DTOs;

namespace Naseejk.core.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<AuthResponseDto>> HandleExternalProviderAsync(string providerName, string idToken, string externalRefreshToken, string deviceId);
        Task<Result<AuthResultDto>> RefreshSessionAsync(string expiredAccessToken, string rawRefreshToken, string deviceId);
        Task<Result<string>> LogoutAsync(string userId, string deviceId);
    }
}
