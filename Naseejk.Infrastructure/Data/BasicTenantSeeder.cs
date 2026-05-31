using Microsoft.EntityFrameworkCore;
using Naseejk.core.Entities;
using Naseejk.core.Interfaces;
using Naseejk.core.Common;

namespace Naseejk.Infrastructure.Data
{
    public class BasicTenantSeeder : IBasicSeeder
    {
        private readonly NaseejkDbContext _context;
        private readonly ISnowflakeIdGenerator _idGenerator;

        public BasicTenantSeeder(NaseejkDbContext context, ISnowflakeIdGenerator idGenerator)
        {
            _context = context;
            _idGenerator = idGenerator;
        }

        public async Task SeedAsync()
        {
            bool hasTenants = await _context.Tenants.AnyAsync();

            if (!hasTenants)
            {
                long uniqueTenantId = _idGenerator.CreateId();
                long defaultAppId = _idGenerator.CreateId();

                var tenant = new Tenant
                {
                    Id = uniqueTenantId,
                    Name = "Naseejk Tenant",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Tenants.Add(tenant);

                var defaultApp = new Application
                {
                    Id = defaultAppId,
                    Name = "Naseejk Identity Hub Admin",
                    Code = "Naseejk_Hub_Admin",
                    ClientIdentifier = Guid.NewGuid().ToString(),
                    ClientSecret = Guid.NewGuid().ToString(),
                    IsActive = true,
                    TenantId = uniqueTenantId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Applications.Add(defaultApp);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Basic tenant and default Application ({defaultAppId}) seeded successfully.");
            }
        }
    }
}