using Naseejk.core.Common;
using Naseejk.core.DTOs;
using Naseejk.core.Entities;
namespace Naseejk.core.Interfaces
{
    public interface IAccountService
    {
        Task<Result<User>> signInAsyncWithGoogle(UserProvisioningDto model);
    }
}
