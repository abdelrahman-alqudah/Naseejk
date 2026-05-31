using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.Entities
{
    public class Permission: BaseEntity
    {
        public long ApplicationId { get; set; }
        // The key is a string unique and we want to protect it from being changed , it's the language that the system speek with database and the code example : user.create 
        public string Key { get; set; } = string.Empty;
        public string? Description { get; set; }
        public virtual Application Application { get; set; } = null!;
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];

    }
}