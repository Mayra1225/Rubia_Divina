using System.ComponentModel.DataAnnotations;

namespace Rubia_Divina.DTOs;

public class DetallePedidoDTO
{
    [Required]
    public int ProductoId { get; set; }

    [Range(1, 100)]
    public int Cantidad { get; set; }
}