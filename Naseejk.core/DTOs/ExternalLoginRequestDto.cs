namespace Naseejk.core.DTOs
{
    public class ExternalLoginRequestDto
    {
        public string Provider { get; set; } = string.Empty; // "GOOGLE" أو "MICROSOFT"
        public string IdToken { get; set; } = string.Empty;
    }
}