namespace Naseejk.core.Entities
{
    public class Application : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? MicrosoftTenantId { get; set; }
        public string ClientIdentifier { get; set; } = Guid.NewGuid().ToString("N");
        public string ClientSecret { get; set; } = Guid.NewGuid().ToString("N");
        public long TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public virtual ICollection<Role> Roles { get; set; } = [];
        public virtual ICollection<Permission> Permissions { get; set; } = [];
        public virtual ICollection<UserExternalIdentitie> UserExternalIdentities { get; set; } = [];
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = [];
        public virtual ICollection<UserAppRole> UserAppRoles { get; set; } = [];
        public virtual ICollection<TokenHistory> TokenHistories { get; set; } = [];
        public virtual ICollection<VirtualLeadConfig> VirtualLeadConfigs { get; set; } = [];

    }
}
