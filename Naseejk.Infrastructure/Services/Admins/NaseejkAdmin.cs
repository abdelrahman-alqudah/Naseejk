using Naseejk.core.Common;
using Naseejk.core.DTOs;
using Naseejk.core.Entities;
using Naseejk.Infrastructure.Services.Admins.Interfaces;
using Naseejk.core.Repositories;
using Microsoft.Extensions.Logging;
using Naseejk.core.Specifications;

namespace Naseejk.Infrastructure.Services.Admins
{
    public class NaseejkAdmin : INaseejkAdmin
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISnowflakeIdGenerator _idGenerator;
        private readonly ILogger<NaseejkAdmin> _logger;

        public NaseejkAdmin(IUnitOfWork unitOfWork, ISnowflakeIdGenerator idGenerator, ILogger<NaseejkAdmin> logger)
        {
            _unitOfWork = unitOfWork;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        public async Task<Result<CreatingTenantResponseDto>> CreateTenantAsync(CreatingTenantRequestDto request)
        {
            if (request is null)
            {
                _logger.LogWarning("CreateTenantAsync called with null request");
                return Result<CreatingTenantResponseDto>.Failure("Request cannot be null");
            }
            _logger.LogInformation("Creating tenant with name: {TenantName}", request.Name);

            var existingTenant = await _unitOfWork.Repository<Tenant>().GetEntityWithSpec(new TenantByNameSpec(request.Name));
            if (existingTenant != null)
            {
                return Result<CreatingTenantResponseDto>.Failure($"A tenant with the name '{request.Name}' already exists.");
            }
            try
            {
                var tenantId = _idGenerator.CreateId();
                var tenant = new Tenant
                {
                    Id = tenantId,
                    Name = request.Name,
                    MicrosoftTenantId = request.MicrosoftTenantId,
                    IsActive = request.IsActive
                };

                await _unitOfWork.Repository<Tenant>().AddAsync(tenant);
                await _unitOfWork.CompleteAsync();
                var response = new CreatingTenantResponseDto
                {
                    Id = tenant.Id,
                    Name = tenant.Name,
                    MicrosoftTenantId = tenant.MicrosoftTenantId,
                    IsActive = tenant.IsActive
                };
                _logger.LogInformation("Tenant created successfully with id: {TenantId}", tenant.Id);
                return Result<CreatingTenantResponseDto>.Success(response);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating tenant with name: {TenantName}", request.Name);
                return Result<CreatingTenantResponseDto>.Failure("An error occurred while creating the tenant. Please try again later.");
            }
        }

        public async Task<Result<CreatingTenantResponseDto>> UpdateTenantAsync(long tenantId, CreatingTenantRequestDto request)
        {
            if (request is null)
            {
                _logger.LogWarning("UpdateTenantAsync called with null request for tenant id: {TenantId}", tenantId);
                return Result<CreatingTenantResponseDto>.Failure("Request cannot be null");
            }

            var tenant = await _unitOfWork.Repository<Tenant>().GetEntityWithSpec(new TenantByIdSpec(tenantId));
            if (tenant == null)
            {
                _logger.LogWarning("UpdateTenantAsync called with non-existing tenant id: {TenantId}", tenantId);
                return Result<CreatingTenantResponseDto>.Failure($"Tenant with id '{tenantId}' not found.");
            }
            var tenantWithSameName = await _unitOfWork.Repository<Tenant>().GetEntityWithSpec(new TenantByNameSpec(request.Name));
            if (tenantWithSameName != null && tenantWithSameName.Id != tenantId)
            {
                _logger.LogWarning("UpdateTenantAsync failed. Name '{TenantName}' is already taken by another tenant.", request.Name);
                return Result<CreatingTenantResponseDto>.Failure($"A tenant with the name '{request.Name}' already exists.");
            }

            tenant.Name = request.Name;
            tenant.MicrosoftTenantId = request.MicrosoftTenantId;
            tenant.IsActive = request.IsActive;

            try
            {
                _unitOfWork.Repository<Tenant>().Update(tenant);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Tenant updated successfully with id: {TenantId}", tenant.Id);
                var response = new CreatingTenantResponseDto
                {
                    Id = tenant.Id,
                    Name = tenant.Name,
                    MicrosoftTenantId = tenant.MicrosoftTenantId,
                    IsActive = tenant.IsActive
                };
                return Result<CreatingTenantResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating tenant with id: {TenantId}", tenantId);
                return Result<CreatingTenantResponseDto>.Failure("An error occurred while updating the tenant. Please try again later.");
            }
        }

        public async Task<Result<bool>> DectivateTenant(long tenantId)
        {
            var tenant = await _unitOfWork.Repository<Tenant>().GetEntityWithSpec(new TenantByIdSpec(tenantId));
            if (tenant == null)
            {
                _logger.LogWarning("DactivateTenant called with non-existing tenant id: {TenantId}", tenantId);
                return Result<bool>.Failure($"Tenant with id '{tenantId}' not found.");
            }

            tenant.IsActive = false;

            try
            {
                _unitOfWork.Repository<Tenant>().Update(tenant);
                await _unitOfWork.CompleteAsync();
                _logger.LogInformation("Tenant deactivated successfully with id: {TenantId}", tenant.Id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to deactivate tenant: {TenantId} because {ErrorMessage}", tenantId, ex.Message);
                return Result<bool>.Failure($"An error occurred");
            }
        }
        public async Task<Result<List<CreatingTenantResponseDto>>> GetAllTenantsAsync()
        {
            try
            {
                var genericTenants = await _unitOfWork.Repository<Tenant>().ListAllAsync();

                var response = genericTenants.Cast<Tenant>().Select(t => new CreatingTenantResponseDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    MicrosoftTenantId = t.MicrosoftTenantId,
                    IsActive = t.IsActive
                }).ToList();

                _logger.LogInformation("Retrieved {TenantCount} tenants successfully.", response.Count);
                return Result<List<CreatingTenantResponseDto>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving tenants.");
                return Result<List<CreatingTenantResponseDto>>.Failure("An error occurred while retrieving tenants.");
            }
        }
    }
}