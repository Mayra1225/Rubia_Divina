using System.ComponentModel.DataAnnotations;

namespace Rubia_Divina.DTOs;

public class ProductoDTO
{
    [Required]
    [StringLength(80)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Categoria { get; set; } = string.Empty;

    [StringLength(250)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0.01, 9999)]
    public decimal Precio { get; set; }

    [Range(0, 10000)]
    public int Stock { get; set; }
}
