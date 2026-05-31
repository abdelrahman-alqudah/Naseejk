using Naseejk.core.Entities;

public class UserExternalIdentitie : BaseEntity
{
    public long UserId { get; set; }
    public long ApplicationId { get; set; }

    public string ExternalId { get; set; } = string.Empty;

    public string IdentitySource { get; set; } = string.Empty; // "MICROSOFT" | "GOOGLE"
    public bool IsVerified { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual Application Application { get; set; } = null!;
}