using Naseejk.core.Entities;

namespace Naseejk.core.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty; 
        public string ExternalRefreshToken { get; set; } = string.Empty; 
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public User? User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}