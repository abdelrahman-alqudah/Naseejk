using Naseejk.core.Common;
using Naseejk.core.DTOs;
using Naseejk.core.Interfaces;
using Naseejk.core.Specifications;
using Naseejk.core.Entities;
using Naseejk.core.Repositories;

namespace Naseejk.Infrastructure.Services
{
    public class GoogleAuthProvider : IExternalAuthProvider
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public GoogleAuthProvider(
            IGoogleAuthService googleAuthService,
            IAccountService accountService,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _googleAuthService = googleAuthService;
            _accountService = accountService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public string ProviderName => "Google";

        public async Task<Result<AuthResponseDto>> AuthenticateAsync(string idToken)
        {
            var googlePayload = await _googleAuthService.VerifyTokenAsync(idToken);

            if (googlePayload == null)
            {
                return Result<AuthResponseDto>.Failure("Invalid Google token.");
            }

            var googleClientId = googlePayload.Audience?.ToString();

            if (string.IsNullOrEmpty(googleClientId))
            {
                return Result<AuthResponseDto>.Failure("Google Token payload is missing the Audience (ClientId).");
            }
            
            // Check if the application is registered in Naseejk by checking the audience (client_id) in the Google payload
            var appSpec = new ApplicationByClientIdSpec(googleClientId);
            var app = await _unitOfWork.Repository<Application>().GetEntityWithSpec(appSpec);

            if (app == null)
                return Result<AuthResponseDto>.Failure("This application is not registered in Naseejk.");

            var userProvisioning = new UserProvisioningDto
            {
                IdToken = idToken, 
                Email = googlePayload.Email,
                Username = googlePayload.Name,
                TenantId = app.TenantId,
                Provider = "Google",
                ProviderKey = googlePayload.Subject,
            };

            var userResult = await _accountService.signInAsyncWithGoogle(userProvisioning);
            if (!userResult.Succeeded)
                return Result<AuthResponseDto>.Failure(userResult.Errors);

            var user = userResult.Data;

            var token = _tokenService.CreateToken(user!, "Google");

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                Token = token,
                Email = user!.Email!,
                Username = user!.UserName!,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
        }
    }
}