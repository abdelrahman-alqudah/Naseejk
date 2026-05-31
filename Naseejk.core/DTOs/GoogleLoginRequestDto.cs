using System.Text.Json.Serialization;

namespace Naseejk.core.DTOs
{
    public class GoogleLoginRequestDto
    {
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; } = string.Empty;
    }
}
