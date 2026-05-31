using Naseejk.core.DTOs;
using Naseejk.core.Interfaces;
using Naseejk.core.Common;

namespace Naseejk.Infrastructure.Services
{
    public class MicrosoftAuthProvider : IExternalAuthProvider
    {
        private readonly IMicrosoftAuthService _msAuthService;
        private readonly IMicrosoftProvisioningService _provisioningService;
        private readonly ITokenService _tokenService;

        public MicrosoftAuthProvider(
            IMicrosoftAuthService msAuthService,
            IMicrosoftProvisioningService provisioningService,
            ITokenService tokenService)
        {
            _msAuthService = msAuthService;
            _provisioningService = provisioningService;
            _tokenService = tokenService;
        }

        public string ProviderName => "Microsoft";

        public async Task<Result<AuthResponseDto>> AuthenticateAsync(string idToken)
        {
            var principal = await _msAuthService.verifyTokenAsync(idToken);
            if (principal == null)
                return Result<AuthResponseDto>.Failure("Invalid Microsoft Token");

            var userResult = await _provisioningService.ProvisionUserAsync(principal);
            if (!userResult.Succeeded)
                return Result<AuthResponseDto>.Failure(userResult.Errors);

            var token = _tokenService.CreateToken(userResult.Data!, "Microsoft");

            return Result<AuthResponseDto>.Success(new AuthResponseDto
            {
                Token = token,
                Email = userResult.Data!.Email!,
                Username = userResult.Data!.UserName!
            });
        }
    }
}