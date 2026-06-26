using Rubia_Divina.DTOs;
using Rubia_Divina.Models;

namespace Rubia_Divina.Interfaces.Services;

public interface IProductoService
{
    Task<List<Producto>> ObtenerPorUsuarioAsync(int usuarioId);

    Task<Producto?> ObtenerUnoAsync(int id, int usuarioId);

    Task<Producto> CrearAsync(
        ProductoDTO dto,
        int usuarioId);

    Task<Producto?> ActualizarAsync(
        int id,
        ProductoDTO dto,
        int usuarioId);

    Task<bool> EliminarAsync(
        int id,
        int usuarioId);
}