using System.Security.Claims;
using Naseejk.core.Common;
using Naseejk.core.Entities;

namespace Naseejk.core.Interfaces
{
    public interface IMicrosoftProvisioningService
    {
        public Task<Result<User>> ProvisionUserAsync(ClaimsPrincipal claimsPrincipal);
    }
}
