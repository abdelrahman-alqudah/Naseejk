using Microsoft.AspNetCore.Identity;
using Naseejk.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.Entities
{
    public class Role : IdentityRole<long>, IBaseEntity
    {
        public long? ApplicationId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        public string? Description { get; set; }
        public bool IsSystemRole { get; set; }
        public virtual Application Application { get; set; } = null!;
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
        public virtual ICollection<UserAppRole> UserAppRoles { get; set; } = [];
        public virtual ICollection<VirtualLeadConfig> VirtualLeadConfigs { get; set; } = [];

    }
}
