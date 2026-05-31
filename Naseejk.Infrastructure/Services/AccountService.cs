using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Naseejk.core.Common;
using Naseejk.core.DTOs;
using Naseejk.core.Repositories;
using Naseejk.core.Entities;
using Naseejk.core.Enums;
using Naseejk.core.Interfaces;
using Naseejk.core.Specifications;

namespace Naseejk.Infrastructure.Services
{
    public class AccountService(
        IUnitOfWork unitOfWork,
        ILogger<AccountService> logger,
        IGoogleAuthService _googleAuthService) : IAccountService
    {

        public async Task<Result<User>> signInAsyncWithGoogle(UserProvisioningDto model)
        {
            logger.LogInformation("Attempting login for {Email} on Tenant {TenantId}", model.Email, model.TenantId);


            var spec = new UserWithEmailSpecification(model.Email, model.TenantId);
            var user = await unitOfWork.Repository<User>().GetEntityWithSpec(spec);

            if (user == null)
            {
                logger.LogWarning("Access Denied: User {Email} is not invited to Tenant {TenantId}", model.Email, model.TenantId);
                return Result<User>.Failure("Error: User not invited to tenant.");
            }

            try
            {
                var payload = await _googleAuthService.VerifyTokenAsync(model.IdToken);
                if (payload == null) throw new UnauthorizedAccessException("Invalid Google Token");

                if (string.IsNullOrEmpty(user.IdentityId))
                {
                    user.IdentityId = payload.Subject;
                    logger.LogInformation("First login for {Email}: Binding Google Subject ID.", model.Email);
                }

                user.UserName = model.Username ?? user.UserName;
                user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
                user.Status = UserStatus.Active;

                unitOfWork.Repository<User>().Update(user);
                await unitOfWork.CompleteAsync();

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during sign-in for {Email}", model.Email);
                return Result<User>.Failure("Error during sign-in.");
            }
        }
    }
}