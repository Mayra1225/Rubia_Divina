using Rubia_Divina.Models;

namespace Rubia_Divina.FactoryMethods;

public class PedidoFactory : IPedidoFactory
{
    public Pedido Crear(
        decimal total,
        int? promocionId,
        List<DetallePedido> detalles)
    {
        return new Pedido
        {
            FechaPedido = DateTime.Now,
            Total = total,
            PromocionId = promocionId,
            Detalles = detalles
        };
    }
}