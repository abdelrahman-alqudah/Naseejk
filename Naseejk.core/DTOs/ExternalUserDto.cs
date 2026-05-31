using System.ComponentModel.DataAnnotations;

namespace Naseejk.core.DTOs;

public class ExternalUserDto
{
    [Required]
    public string Provider { get; set; } = string.Empty;
    
    [Required]
    public string TokenId { get; set; } = string.Empty;


}