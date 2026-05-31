using Microsoft.Extensions.Configuration;
using Naseejk.core.Common;
using Naseejk.core.DTOs;
using Naseejk.core.Entities;
using Naseejk.core.Interfaces;
using Naseejk.core.Repositories;

namespace Naseejk.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IEnumerable<IExternalAuthProvider> _authProviders;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _uof;
        private readonly ISessionManager _sessionManager;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public IdentityService(IEnumerable<IExternalAuthProvider> authProviders, ITokenService tokenService, IUnitOfWork uof, ISessionManager sessionManager,IConfiguration configuration,HttpClient httpClient)
        {
            _authProviders = authProviders;
            _tokenService = tokenService;
            _uof = uof;
            _sessionManager = sessionManager;
            _configuration = configuration;
            _httpClient = httpClient;

        }

        public async Task<Result<AuthResponseDto>> HandleExternalProviderAsync(string providerName, string idToken, string externalRefreshToken, string deviceId)
        {
            var provider = _authProviders.FirstOrDefault(p =>
                p.ProviderName.Equals(providerName, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
                return Result<AuthResponseDto>.Failure($"Provider '{providerName}' not supported.");

            var authResult = await provider.AuthenticateAsync(idToken);
            if (!authResult.Succeeded) return authResult;

            var userId = authResult.Data?.User?.Id.ToString();

            var refreshToken = await GenerateAndSaveSession(userId!, deviceId);

            if (providerName.Equals("Microsoft", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(externalRefreshToken))
            {
              await _sessionManager.SaveExternalProviderTokenAsync(userId!, providerName, "RefreshToken", externalRefreshToken);
            }

            authResult.Data!.RefreshToken = refreshToken;

            return authResult;
        }
        private async Task<string> GenerateAndSaveSession(string userId, string deviceId)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _sessionManager.CreateSessionAsync(userId, refreshToken, deviceId, TimeSpan.FromDays(7));
            return refreshToken;
        }

        public async Task<Result<AuthResultDto>> RefreshSessionAsync(string expiredAccessToken, string rawRefreshToken, string deviceId)
        {

            var userId = _tokenService.GetUserIdFromExpiredToken(expiredAccessToken);
            if (string.IsNullOrEmpty(userId))
            {
                return Result<AuthResultDto>.Failure("Invalid access token.");
            }

            var user = await _uof.Repository<User>().GetByIdAsync(long.Parse(userId));

            if (user == null)
            {
                await _sessionManager.RevokeSessionAsync(userId, deviceId);
                return Result<AuthResultDto>.Failure("User not found.");
            }
            var isValidSession = await _sessionManager.ValidateSessionAsync(userId, deviceId, rawRefreshToken);
            if (!isValidSession)
            {
                return Result<AuthResultDto>.Failure("Invalid session or refresh token.");
            }
            var MicrosoftRefreshToken = await _sessionManager.GetExternalProviderRefreshTokenAsync(userId, "Microsoft");
            if (!string.IsNullOrEmpty(MicrosoftRefreshToken))
            {
                var isMicrosoftSessionValid = await ValidateMicrosoftSessionAsync(MicrosoftRefreshToken);
                if (!isMicrosoftSessionValid)
                {
                    await LogoutAsync(userId, deviceId);
                    return Result<AuthResultDto>.Failure("Invalid Microsoft session. Account may be disabled.");
                }
            }

            
            var newAccessToken = _tokenService.CreateToken(user, "Naseejk");
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _sessionManager.CreateSessionAsync(userId, newRefreshToken, deviceId, TimeSpan.FromDays(7));
            return Result<AuthResultDto>.Success(new AuthResultDto
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        public async Task<Result<string>> LogoutAsync(string userId, string deviceId)
        {

            await _sessionManager.RevokeSessionAsync(userId, deviceId);
            return Result<string>.Success("Session revoked successfully.");
        }

        public async Task<bool> ValidateMicrosoftSessionAsync(string savedMicrosoftRefreshToken)
        {
            var tenantId = _configuration["MicrosoftAuth:tenantId"];
            var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            var requestData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _configuration["MicrosoftAuth:clientId"]!),
                new KeyValuePair<string, string>("client_secret", _configuration["MicrosoftAuth:clientSecret"]!),
                new KeyValuePair<string, string>("refresh_token", savedMicrosoftRefreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token") 
            });

            var response = await _httpClient.PostAsync(tokenEndpoint, requestData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                if (errorContent.Contains("AADSTS50057"))
                {
                    Console.WriteLine("[Naseejk Security]: Microsoft token validation indicates user account is disabled");
                    return false; 
                }

                return false; 
            }

            return true;
        }
    }
}