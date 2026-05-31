using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.Entities
{
    public class VirtualLeadConfig:BaseEntity
    {
        public long ApplicationId { get; set; }
        public long TargetRoleId { get; set; }
        public long? PrimaryUserId { get; set; }
        public long SubstituteUserId { get; set; }
        public string? Reason { get; set; } 
        public DateTime? StartAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? EndsAtUtc { get; set; } = DateTime.MaxValue;
        public bool IsEnabled { get; set; }
        public virtual Application Application { get; set; } = null!;
         public virtual Role TargetRole { get; set; } = null!;
        public virtual User? PrimaryUser { get; set; }
        public virtual User? User { get; set; }
        public virtual User SubstituteUser { get; set; } = null!;

    }
}
