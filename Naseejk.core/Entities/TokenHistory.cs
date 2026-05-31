using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.Entities
{ 
    public class TokenHistory: BaseEntity
    {
        public long UserId { get; set; }
        public long ApplicationId { get; set; }
        public string? JwtId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime IssuedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAtUtc { get; set; } = DateTime.UtcNow.AddHours(1);
        public DateTime? RevokedAtUtc { get; set; } = null;
        public string? IpAddress { get; set; } 
        public string? CorrelationId { get; set; }
        public string? TraceId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Application Application { get; set; } = null!;
    }
}
