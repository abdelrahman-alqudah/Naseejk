using Microsoft.AspNetCore.Identity;
using Naseejk.core.Interfaces;
using Naseejk.core.Enums;

namespace Naseejk.core.Entities
{
    public class User : IdentityUser<long>, IBaseEntity
    {
        
        public string IdentityId { get; set; } = Guid.NewGuid().ToString();
        public long TenantId { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Pending;
        public virtual ICollection<UserExternalIdentitie> UserExternalIdentities { get; set; } = [];
        public virtual ICollection<UserAppRole> UserAppRoles { get; set; } = [];
        public virtual ICollection<TokenHistory> TokenHistories { get; set; } = [];
        public Tenant Tenant { get; set; } = null!;
        public virtual ICollection<VirtualLeadConfig> VirtualLeadConfigsAsPrimary { get; set; } = [];
        public virtual ICollection<VirtualLeadConfig> VirtualLeadConfigsAsSubstitute { get; set; } = [];
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = [];
    }
}