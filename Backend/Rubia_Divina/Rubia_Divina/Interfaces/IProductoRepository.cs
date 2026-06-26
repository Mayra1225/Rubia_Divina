using Rubia_Divina.Models;

namespace Rubia_Divina.Interfaces;

public interface IProductoRepository
{
    Task<List<Producto>> ObtenerPorUsuarioAsync(int usuarioId);

    Task<Producto?> ObtenerUnoAsync(
        int id,
        int usuarioId);

    Task AgregarAsync(Producto producto);

    Task GuardarCambiosAsync();

    void Eliminar(Producto producto);
}