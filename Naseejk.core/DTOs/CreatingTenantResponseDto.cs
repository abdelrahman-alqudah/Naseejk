using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.DTOs
{
    public class CreatingTenantResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? MicrosoftTenantId { get; set; }
        public bool IsActive { get; set; }
    }
}
