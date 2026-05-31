using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.Entities
{
    public class RolePermission : BaseEntity
    {
        public long RoleId {  get; set; }
        public long PermissionId { get; set; }
        public virtual Role Role { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
}