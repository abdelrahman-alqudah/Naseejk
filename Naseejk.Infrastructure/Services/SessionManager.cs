using Microsoft.Extensions.Caching.Distributed;
using Naseejk.core.Common;
using Naseejk.core.DTOs;
using Naseejk.core.Interfaces;
using Naseejk.core.Entities;
using Naseejk.core.Repositories;
using System.Security.Cryptography.X509Certificates;


namespace Naseejk.Infrastructure.Services
{
    public class SessionManager : ISessionManager
    {
        private readonly IDistributedCache _cache;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _uof;

        public SessionManager(IDistributedCache cache, ITokenService tokenService, IUnitOfWork uof)
        {
            _cache = cache;
            _tokenService = tokenService;
            _uof = uof;
        }

        public async Task CreateSessionAsync(string userId, string refreshToken, string deviceId, TimeSpan sessionDuration)
        {
            // Hash the refresh token before storing it to enhance security in case of cache breaches
            var hashedToken = _tokenService.HashToken(refreshToken);
            // Create a unique key for the session using user ID and device ID to allow multiple sessions per user but i destroy Multi-Device Login each time i login from a new device
            var sessionKey = $"session:{userId}:{deviceId}";

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = sessionDuration
            };
            // Store the hashed refresh token in the cache with an expiration time in Redis
            await _cache.SetStringAsync(sessionKey, hashedToken, options);
        }

        public async Task<bool> ValidateSessionAsync(string userId, string deviceId, string rawRefreshToken)
        {
            var sessionKey = $"session:{userId}:{deviceId}";
            var storedHash = await _cache.GetStringAsync(sessionKey);

            if (storedHash == null) return false;

            var incomingHash = _tokenService.HashToken(rawRefreshToken);
            return storedHash == incomingHash;
        }

        public async Task RevokeSessionAsync(string userId, string deviceId)
        {
            var sessionKey = $"session:{userId}:{deviceId}";

            await _cache.RemoveAsync(sessionKey);
        }

        public async Task<string> GetExternalProviderRefreshTokenAsync(string userId, string providerName)
        {
            var userExternalToken = await _uof.Repository<UserExternalToken>()
                .FirstOrDefaultAsync(t =>
            t.UserId == userId &&
            t.Provider == providerName &&
            t.TokenName == "RefreshToken");

            if (userExternalToken == null)
            {
                Console.WriteLine($"[Naseejk Security Warning]: No {providerName} Refresh Token found in DB for user {userId}.");
                return null!;
            }

            return userExternalToken.TokenValue;
        }
        public async Task SaveExternalProviderTokenAsync(string userId, string providerName, string tokenName, string tokenValue)
        {
            var userExternalToken = await _uof.Repository<UserExternalToken>()
                .FirstOrDefaultAsync(t =>
            t.UserId == userId &&
            t.Provider == providerName &&
            t.TokenName == tokenName);
            if (userExternalToken == null)
            {
                userExternalToken = new UserExternalToken
                {
                    UserId = userId,
                    Provider = providerName,
                    TokenName = tokenName,
                    TokenValue = tokenValue
                };
                await _uof.Repository<UserExternalToken>().AddAsync(userExternalToken);
            }
            else
            {
                userExternalToken.TokenValue = tokenValue;
                _uof.Repository<UserExternalToken>().Update(userExternalToken);
            }
            await _uof.CompleteAsync();
        }
    }
}
