namespace Naseejk.core.DTOs;

public class UserProvisioningDto
{
    public string IdToken { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string ProviderKey { get; set; } = string.Empty;
    public long TenantId { get; set; }
    public string? PhoneNumber { get; set; } = string.Empty;

}