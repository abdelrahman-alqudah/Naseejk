using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.core.DTOs
{
    public class AddApplicationResDto
    {
        public long Id { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string ClientIdentifier { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
