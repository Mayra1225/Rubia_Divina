using Rubia_Divina.Models;
using System.Text.Json.Serialization;

public class Pedido
{
    public int Id { get; set; }

    public DateTime FechaPedido { get; set; }

    public decimal Total { get; set; }

    public int? PromocionId { get; set; }

    public Promocion? Promocion { get; set; }

    public ICollection<DetallePedido> Detalles { get; set; }
        = new List<DetallePedido>();
}