using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Naseejk.core.Interfaces;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class MicrosoftAuthService : IMicrosoftAuthService
{
    private readonly ILogger<MicrosoftAuthService> _logger;

    private static readonly ConcurrentDictionary<string, ConfigurationManager<OpenIdConnectConfiguration>> _managers = new();

    public MicrosoftAuthService(ILogger<MicrosoftAuthService> logger)
    {
        _logger = logger;
    }

    public async Task<ClaimsPrincipal?> verifyTokenAsync(string idToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(idToken)) return null;

            var jwtToken = handler.ReadJwtToken(idToken);
            var tenantId = jwtToken.Claims.FirstOrDefault(c => c.Type == "tid")?.Value;
            var clientId = jwtToken.Claims.FirstOrDefault(c => c.Type == "aud")?.Value;

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId)) return null;

            var configManager = _managers.GetOrAdd(tenantId, tid =>
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"https://login.microsoftonline.com/{tid}/v2.0/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever()));

            var openIdConfig = await configManager.GetConfigurationAsync();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = openIdConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = clientId,
                IssuerSigningKeys = openIdConfig.SigningKeys,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            return handler.ValidateToken(idToken, validationParameters, out _);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token Validation Failed: {Message}", ex.Message);
            return null;
        }
    }
}