using System.ComponentModel.DataAnnotations;

namespace Naseejk.core.DTOs
{
    public class MicrosoftLoginRequestDto
    {
        [Required(ErrorMessage = "IdToken is mandatory.")]
        public string IdToken { get; set; } = string.Empty;
    }
}