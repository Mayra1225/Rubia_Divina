using Rubia_Divina.Models;

namespace Rubia_Divina.FactoryMethods;

public interface IPedidoFactory
{
    Pedido Crear(
        decimal total,
        int? promocionId,
        List<DetallePedido> detalles
    );
}