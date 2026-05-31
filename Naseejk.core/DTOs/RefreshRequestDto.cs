using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.DTOs
{
    public class RefreshRequestDto
    {
        public string ExpiredToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
