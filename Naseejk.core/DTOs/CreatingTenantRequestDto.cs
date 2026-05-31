using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.DTOs
{
    public class CreatingTenantRequestDto
    {
        public required string Name { get; set; } 
        public string? MicrosoftTenantId { get; set; } 
        public bool IsActive { get; set; }
    }
}
