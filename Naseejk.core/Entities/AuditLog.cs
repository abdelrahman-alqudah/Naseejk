using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.Entities
{
    public class AuditLog: BaseEntity
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public long? ApplicationId {  get; set; }
        public long? UserId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string PermissionKey { get; set; } = string.Empty;
        public string? Reason { get; set; } 
        public long? CorrelationId { get; set; }
        public string? TraceId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? MetadataJson { get; set; }
        public virtual Application? Application { get; set; }
        public virtual User? User { get; set; }
    }
}
