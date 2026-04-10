using System.ComponentModel.DataAnnotations;

namespace Rubia_Divina.DTOs;

public class RegisterDTO
{
    [Required]
    [StringLength(80)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
