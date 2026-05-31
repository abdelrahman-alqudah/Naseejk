using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Naseejk.core.Interfaces;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly string _clientId;
    private readonly ILogger<GoogleAuthService> _logger;
    public GoogleAuthService(IConfiguration configuration, ILogger<GoogleAuthService> logger)
    {
        _logger = logger;
        _clientId = configuration["Google:ClientId"]
                    ?? throw new InvalidOperationException("Google ClientId missing!");
    }
    public async Task<GoogleJsonWebSignature.Payload?> VerifyTokenAsync(string idToken)
    {

        if (string.IsNullOrEmpty(_clientId))
        {
            _logger.LogCritical("Google ClientId is missing in configuration!");
            throw new InvalidOperationException("Google Authentication is not configured.");
        }

        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { _clientId }
        };

        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Google Token validation failed.");
            return null;
        }
    }
}