using Rubia_Divina.Models;

namespace Rubia_Divina.Interfaces.Repositories;

public interface IProductoRepository
{
    Task<List<Producto>> ObtenerPorUsuarioAsync(int usuarioId);

    Task<Producto?> ObtenerUnoAsync(int id, int usuarioId);

    Task AgregarAsync(Producto producto);

    Task GuardarCambiosAsync();

    Task EliminarAsync(Producto producto);

    Task<Categoria?> ObtenerCategoriaAsync(int categoriaId);

    Task<List<Producto>> ObtenerPorCategoriaAsync(
    int categoriaId,
    int usuarioId);
}