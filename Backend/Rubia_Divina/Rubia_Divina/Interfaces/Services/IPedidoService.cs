using Rubia_Divina.DTOs;
using Rubia_Divina.Models;

namespace Rubia_Divina.Interfaces.Services;

public interface IPedidoService
{
    Task<List<Pedido>> ObtenerTodosAsync();

    Task<Pedido?> ObtenerUnoAsync(int id);

    Task<Pedido> CrearAsync(PedidoDTO dto);

    Task<Pedido?> ActualizarAsync(
        int id,
        PedidoDTO dto);

    Task<bool> EliminarAsync(int id);
}