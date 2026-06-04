using System.ComponentModel.DataAnnotations;

namespace Rubia_Divina.DTOs;

public class PedidoDTO
{
    public int? PromocionId { get; set; }

    [Required]
    public List<DetallePedidoDTO> Detalles { get; set; }
        = new();
}