using Rubia_Divina.Models;

namespace Rubia_Divina.Interfaces.Repositories;

public interface IPedidoRepository
{
    Task<List<Pedido>> ObtenerTodosAsync();

    Task<Pedido?> ObtenerUnoAsync(int id);

    Task<Producto?> ObtenerProductoAsync(int productoId);

    Task<Promocion?> ObtenerPromocionAsync(int promocionId);

    Task AgregarPedidoAsync(Pedido pedido);

    Task GuardarCambiosAsync();

    Task EliminarPedidoAsync(Pedido pedido);

    Task EliminarDetallesAsync(IEnumerable<DetallePedido> detalles);
}