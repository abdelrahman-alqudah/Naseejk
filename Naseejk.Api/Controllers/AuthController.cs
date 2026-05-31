using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naseejk.core.DTOs;
using Naseejk.core.Interfaces; 
using System.Diagnostics;
using System.Security.Claims;

namespace Naseejk.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IIdentityService identityService) : ControllerBase
    {
        [HttpPost("external-login")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginRequestDto request, [FromHeader(Name = "X-Device-Id")] string deviceId, [FromHeader(Name = "X-External-Refresh-Token")] string? externalRefreshToken)
        {
            var result = await identityService.HandleExternalProviderAsync(request.Provider, request.IdToken, externalRefreshToken!, deviceId);
            Console.WriteLine(externalRefreshToken);

            if (!result.Succeeded) return BadRequest(result);

            return Ok(result.Data);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
        [FromHeader(Name = "X-Device-Id")] string deviceId,
        [FromBody] RefreshRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return BadRequest(new { message = "Security Violation: Device fingerprint is missing." });
            }

            var result = await identityService.RefreshSessionAsync(request.ExpiredToken, request.RefreshToken, deviceId);

            if (!result.Succeeded) return Unauthorized(result);

            return Ok(result.Data);
        }

        [HttpGet("check-session")]
        [Authorize]
        public IActionResult CheckSession() => Ok(true);

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value
                         ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                var claimsList = string.Join(", ", User.Claims.Select(c => $"{c.Type}:{c.Value}"));
                Console.WriteLine($"[Naseejk Debug]: Claims present: {claimsList}");
                return BadRequest(new { message = "User context missing from token." });
            }

            var deviceId = Request.Headers["X-Device-Id"].ToString();
            if (string.IsNullOrEmpty(deviceId))
            {
                return BadRequest(new { message = "Device ID missing." });
            }

            var result = await identityService.LogoutAsync(userId, deviceId);

            if (!result.Succeeded) return Unauthorized();

            Response.Cookies.Delete("naseejk_auth");
            Response.Cookies.Delete("naseejk_refresh");

            return Ok(new { message = "Logged out successfully" });
        }
    }
}