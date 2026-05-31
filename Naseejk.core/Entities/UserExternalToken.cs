
namespace Naseejk.core.Entities
{
    public class UserExternalToken : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string TokenName { get; set; } = string.Empty;
        public string TokenValue { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}