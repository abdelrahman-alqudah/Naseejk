using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.DTOs
{
    public class AddApplicationReqDto
    {
        public required string ApplicationName { get; set; }
        public bool IsActive { get; set; }
        public string? MicrosoftTenantId { get; set; }
        
    }
}
