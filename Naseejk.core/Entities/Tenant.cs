using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.Entities
{
    public class Tenant: AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? MicrosoftTenantId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
