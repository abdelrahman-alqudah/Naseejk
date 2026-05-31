using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.DTOs
{
    public class UpdateApplicationReqDto
    {
        public string ApplicationName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
