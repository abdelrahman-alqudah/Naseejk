using Naseejk.core.Common;
using Naseejk.core.DTOs;
namespace Naseejk.core.Interfaces
{
    public interface ISessionManager
    {
        Task CreateSessionAsync(string userId, string refreshToken, string deviceId, TimeSpan sessionDuration);
        Task<bool> ValidateSessionAsync(string userId, string deviceId, string rawRefreshToken);
        //Task<Result<AuthResultDto>> RefreshSessionAsync(string expiredAccessToken, string rawRefreshToken, string deviceId);
        Task RevokeSessionAsync(string userId, string deviceId);
        Task<string> GetExternalProviderRefreshTokenAsync(string userId, string providerName);
        Task SaveExternalProviderTokenAsync(string userId, string providerName, string tokenName, string tokenValue);
    }
}
