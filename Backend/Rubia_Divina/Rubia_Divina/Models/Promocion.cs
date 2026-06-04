using System.Text.Json.Serialization;

namespace Rubia_Divina.Models;

public class Promocion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public decimal Descuento { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public bool Activa { get; set; }

    [JsonIgnore]
    public ICollection<Pedido> Pedidos { get; set; }
        = new List<Pedido>();
}