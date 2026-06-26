using Rubia_Divina.DTOs;
using Rubia_Divina.Models;

namespace Rubia_Divina.FactoryMethods;

public class ProductoFactory : IProductoFactory
{
    public Producto Crear(
        ProductoDTO dto,
        int usuarioId)
    {
        return new Producto
        {
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock,
            UsuarioId = usuarioId,
            CategoriaId = dto.CategoriaId,
            ImagenUrl = dto.ImagenUrl,
            FechaCreacion = DateTime.Now
        };
    }
}