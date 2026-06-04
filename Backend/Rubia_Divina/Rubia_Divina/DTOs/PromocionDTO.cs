using System.ComponentModel.DataAnnotations;

namespace Rubia_Divina.DTOs;

public class PromocionDTO
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    [Range(0, 100)]
    public decimal Descuento { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public bool Activa { get; set; }
}