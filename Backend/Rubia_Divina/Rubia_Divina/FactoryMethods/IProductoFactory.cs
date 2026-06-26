using Rubia_Divina.DTOs;
using Rubia_Divina.Models;

namespace Rubia_Divina.FactoryMethods;

public interface IProductoFactory
{
    Producto Crear(
        ProductoDTO dto,
        int usuarioId
    );
}