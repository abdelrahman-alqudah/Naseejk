using Naseejk.core.Entities;

namespace Naseejk.core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user, string provider);
        string GetUserIdFromExpiredToken(string token);
        string HashToken(string token);
        string GenerateRefreshToken();
    }
}
