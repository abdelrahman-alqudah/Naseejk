namespace Naseejk.core.Entities
{
    public class UserAppRole : BaseEntity
    {
        public long UserId { get; set; }
        public long ApplicationId { get; set; }
        public long RoleId { get; set; }
        public long? AssignedByUserId { get; set; }
        public DateTime AssignAtUtc { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual Application Application { get; set; } = null!;
    }
}
