using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Naseejk.core.Entities; 
using System.Reflection;

namespace Naseejk.Infrastructure.Data;

public class NaseejkDbContext : IdentityDbContext<User, Role, long>
{
    public NaseejkDbContext(DbContextOptions<NaseejkDbContext> options) : base(options) { }

    public DbSet<core.Entities.Application> Applications => Set<core.Entities.Application>();
    public DbSet<User> NaseejkUsers => Set<User>();
    public DbSet<core.Entities.Role> NaseejkRoles => Set<core.Entities.Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserAppRole> UserAppRoles => Set<UserAppRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserExternalIdentitie> UserExternalIdentities => Set<UserExternalIdentitie>();
    public DbSet<TokenHistory> TokenHistories => Set<TokenHistory>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<VirtualLeadConfig> VirtualLeadConfigs => Set<VirtualLeadConfig>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<UserExternalToken> UserExternalTokens => Set<UserExternalToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entity.FindProperty("Id");
            // For entities with a long Id, configure it to not be value generated (i.e., not auto-incremented)
            if (idProperty != null && idProperty.ClrType == typeof(long))
            {
                modelBuilder.Entity(entity.ClrType)
                    .Property("Id")
                    .ValueGeneratedNever();
            }
        }
        modelBuilder.UseOpenIddict();


        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}