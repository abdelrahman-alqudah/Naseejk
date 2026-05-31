using Naseejk.core.Entities;
using Naseejk.core.Common;
using Naseejk.core.DTOs;
using System.Fabric.Query;
namespace Naseejk.Infrastructure.Services.Admins.Interfaces
{
    public interface INaseejkAdmin
    {
        Task<Result<CreatingTenantResponseDto>> CreateTenantAsync(CreatingTenantRequestDto request);
        Task<Result<CreatingTenantResponseDto>> UpdateTenantAsync(long tenantId, CreatingTenantRequestDto request);
        Task<Result<bool>> DectivateTenant(long tenantId);
        Task<Result<List<CreatingTenantResponseDto>>> GetAllTenantsAsync();
        //Task<Result<PagedList<CreatingTenantResponseDto>>> GetTenantUsersAsync(long tenantId, int pageNumber, int pageSize);
    }
}
