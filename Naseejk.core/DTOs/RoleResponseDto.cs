using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.DTOs
{
    public class RoleResponseDto
    {
        public required long Id { get; set; }
        public required string Name { get; set; } 
        public string? Description { get; set; } 
        public required bool IsSystemRole { get; set; }
    }
}
