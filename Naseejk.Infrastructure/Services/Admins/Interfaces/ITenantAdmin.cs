using Naseejk.core.Common;
using Naseejk.core.DTOs;
using Naseejk.core.Entities;
namespace Naseejk.Infrastructure.Services.Admins.Interfaces
{
    public interface ITenantAdmin
    {
        Task<Result<AddApplicationResDto>>AddApplicationAsync(long tenantId,AddApplicationReqDto request);
        Task<Result<ApplicationResponseDto>>UpdateApplicationAsync(long tenantId,long applicationId,UpdateApplicationReqDto request);
        Task<Result<bool>>DeactivateApplicationAsync(long tenantId,long applicationId);
        //==================================================================================
        Task<Result<AddEmployeeResDto>>AddEmployeeAsync(long tenantId,AddEmployeeReqDto request);
        Task<Result<EmployeeResponseDto>>UpdateEmployeeAsync(long tenantId,long employeeId,UpdateEmployeeReqDto request);
        Task<Result<bool>>DeactivateEmployeeAsync(long tenantId,long employeeId);
        //==================================================================================
        Task<Result<RoleResponseDto>>AddRoleAsync(long tenantId,AddRoleReqDto request);
        Task<Result<RoleResponseDto>>UpdateRoleAsync(long tenantId,long roleId,UpdateRoleReqDto request);
        //==================================================================================
         Task<Result<bool>>AssignRoleToEmployeeAsync(long tenantId,long employeeId,long roleId);
         Task<Result<bool>>RemoveRoleFromEmployeeAsync(long tenantId,long employeeId,long roleId);
        //==================================================================================
        Task<Result<bool>> UpdateRolePermissionsAsync(long tenantId, long roleId, List<long> permissionIds);
        Task<Result<bool>> RemovePermissionFromRoleAsync(long tenantId, long roleId, long permissionId);
        Task<Result<bool>> UpdatePermissionActionsAsync(long tenantId, long permissionId, List<long> actionIds);
        Task<Result<bool>> RemoveActionFromPermissionAsync(long tenantId, long permissionId, long actionId);

    }
}
