using Naseejk.core.Enums;

namespace Naseejk.core.DTOs
{
    public class AddEmployeeResDto
    {
        public required long Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required UserStatus Status { get; set; }
    }
}
