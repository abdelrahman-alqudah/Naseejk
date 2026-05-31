using Naseejk.core.Enums;
namespace Naseejk.core.DTOs
{
    public class AddEmployeeReqDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; } = string.Empty;
        public required UserStatus Status { get; set; }
    }
}
