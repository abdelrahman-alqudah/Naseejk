using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Naseejk.core.Common;
using Naseejk.core.DTOs;
using Naseejk.core.Entities;
using Naseejk.core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Naseejk.Infrastructure.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string CreateToken(User user, string? provider)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Email, user!.Email!),
                new (JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new ("institution", user.Tenant?.Name ?? "Naseejk Global"),

                new ("grant_type" , "urn:ietf:params:oauth:grant-type:token-exchange"),
                new ("tenant_id", user.TenantId.ToString()),
                new ("idp", provider!)
            };

            if (user.UserAppRoles?.Any() == true)
            {
                var roleClaims = user.UserAppRoles
                    .Where(ur => ur.Role != null)
                    .Select(ur => new Claim(ClaimTypes.Role, ur.Role!.Name!));

                claims.AddRange(roleClaims);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // the refresh token is a long random string that is used to obtain a new access token when the current access token expires. It is typically stored securely on the client side and sent to the server when requesting a new access token. The refresh token should be unique, hard to guess, and have a longer expiration time than the access token to ensure that it can be used to obtain new access tokens without requiring the user to re-authenticate frequently.
        public string GenerateRefreshToken()
        {
            // create a byte array to hold the random data for the refresh token
            var randomBytes = new byte[64];

            // use a secure random number generator to create the refresh token 
            using var rng = RandomNumberGenerator.Create();
            // fill the byte array with random data to ensure the refresh token is unique and hard to guess for attackers
            rng.GetBytes(randomBytes);

            //encrypt the token before sending it to the client
            return Convert.ToBase64String(randomBytes);
        }


        public string GetUserIdFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token format");
            }

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }

        public string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);


        }
    }
}