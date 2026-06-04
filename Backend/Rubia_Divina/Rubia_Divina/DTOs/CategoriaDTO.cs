using System.ComponentModel.DataAnnotations;

namespace Rubia_Divina.DTOs;

public class CategoriaDTO
{
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;
}