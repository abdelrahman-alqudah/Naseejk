using Microsoft.Extensions.Logging;
using Naseejk.core.Common;
using Naseejk.core.Entities;
using Naseejk.core.Interfaces;
using Naseejk.core.Repositories;
using Naseejk.core.Specifications;
using System.Security.Claims;


namespace Naseejk.Infrastructure.Services
{

    public class MicrosoftProvisioningService : IMicrosoftProvisioningService
    {
        private readonly ILogger<MicrosoftProvisioningService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISnowflakeIdGenerator _idGenerator;

        public MicrosoftProvisioningService( ILogger<MicrosoftProvisioningService> logger, IUnitOfWork unitOfWork, ISnowflakeIdGenerator idGenerator)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _idGenerator = idGenerator;
        }
        public async Task<Result<User>> ProvisionUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var clientId = claimsPrincipal.FindFirst("aud")?.Value
               ?? claimsPrincipal.FindFirst("appid")?.Value
               ?? claimsPrincipal.FindFirst("http://schemas.microsoft.com/identity/claims/appid")?.Value;

            var tenantId = claimsPrincipal.FindFirst("tid")?.Value
                           ?? claimsPrincipal.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

            var microsoftId = claimsPrincipal.FindFirst("oid")?.Value
                              ?? claimsPrincipal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                              ?? claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId))
            {
                _logger.LogError("Critical: Tenant ID (tid) or Client ID (appid) is missing from the claims.");
                return Result<User>.Failure("Invalid token: Tenant ID (tid) or Client ID (appid) not found.");
            }
            var tenantSpec = new TenantWithMicrosoftIdSpecification(tenantId);



            if (string.IsNullOrEmpty(microsoftId))
            {
                _logger.LogError("Critical: Microsoft Object ID (oid) is missing from the claims for Tenant: {TenantId}", tenantId);
                return Result<User>.Failure("Invalid token: Microsoft Identity (oid) not found.");
            }

            var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value
                        ?? claimsPrincipal.FindFirst("preferred_username")?.Value;
            var fullName = claimsPrincipal.FindFirst("name")?.Value ?? "Naseejk User";

            if (string.IsNullOrEmpty(email)|| string.IsNullOrEmpty(fullName)){
                return Result<User>.Failure("Invalid credentials: Email or Full Name not found.");

            }
            var appSpec = new ApplicationWithMicrosoftIdSpecification(clientId);
            var application = await _unitOfWork.Repository<Application>().GetEntityWithSpec(appSpec);
            if (application == null)
                return Result<User>.Failure("This Client ID is not registered in Naseejk.");
                
            var tenant = await _unitOfWork.Repository<Tenant>().GetEntityWithSpec(tenantSpec);

            if (tenant == null)
            {
                return Result<User>.Failure("The Company isn't registered in Naseejk");
            }

            if (application.MicrosoftTenantId != tenant.MicrosoftTenantId )
                return Result<User>.Failure("Security Alert: Application does not belong to this Tenant.");

            var externalSpec = new ExternalIdentitySpecification(microsoftId, application.Id);
            var externalIdentity = await _unitOfWork.Repository<UserExternalIdentitie>().GetEntityWithSpec(externalSpec);

            if (externalIdentity != null)
            {
                return Result<User>.Success(externalIdentity.User);
            }
            

            var userSpec = new UserWithEmailSpecification(email, tenant.Id);
            var user = await _unitOfWork.Repository<User>().GetEntityWithSpec(userSpec);

            if (user == null)
            {
                user = new User {Id = _idGenerator.CreateId(), Email = email!, UserName = fullName, UserStatus = "Active", TenantId = tenant.Id };
                await _unitOfWork.Repository<User>().AddAsync(user);
            }

            var newExternalIdentity = new UserExternalIdentitie
            {
                UserId = user.Id,
                ApplicationId = application.Id,
                ExternalId = microsoftId, 
                IdentitySource = "MICROSOFT",
                IsVerified = true,
            };

            await _unitOfWork.Repository<UserExternalIdentitie>().AddAsync(newExternalIdentity);
            await _unitOfWork.CompleteAsync();

            return Result<User>.Success(user);
        }
    }
}
