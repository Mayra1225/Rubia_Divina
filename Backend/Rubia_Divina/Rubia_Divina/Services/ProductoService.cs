using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;
using Rubia_Divina.Models;

namespace Rubia_Divina.Services;

public class ProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerUnoAsync(int id, int usuarioId)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);
    }

    public async Task<Producto> CrearAsync(ProductoDTO dto, int usuarioId)
    {
        if (dto.Precio <= 0)
        {
            throw new Exception("El precio debe ser mayor a 0.");
        }

        if (dto.Stock < 0)
        {
            throw new Exception("El stock no puede ser negativo.");
        }

        var categoriaExiste = await _context.Categorias
            .AnyAsync(c => c.Id == dto.CategoriaId);

        if (!categoriaExiste)
        {
            throw new Exception("La categoría seleccionada no existe.");
        }

        var producto = new Producto
        {
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock,
            UsuarioId = usuarioId,
            CategoriaId = dto.CategoriaId
        };

        _context.Productos.Add(producto);

        await _context.SaveChangesAsync();

        return producto;
    }

    public async Task<Producto?> ActualizarAsync(int id, ProductoDTO dto, int usuarioId)
    {
        var producto = await ObtenerUnoAsync(id, usuarioId);

        if (producto is null)
        {
            return null;
        }

        if (dto.Precio <= 0)
        {
            throw new Exception("El precio debe ser mayor a 0.");
        }

        if (dto.Stock < 0)
        {
            throw new Exception("El stock no puede ser negativo.");
        }

        var categoriaExiste = await _context.Categorias
            .AnyAsync(c => c.Id == dto.CategoriaId);

        if (!categoriaExiste)
        {
            throw new Exception("La categoría seleccionada no existe.");
        }

        producto.Nombre = dto.Nombre.Trim();
        producto.Descripcion = dto.Descripcion.Trim();
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;
        producto.CategoriaId = dto.CategoriaId;

        await _context.SaveChangesAsync();

        return producto;
    }

    public async Task<bool> EliminarAsync(int id, int usuarioId)
    {
        var producto = await ObtenerUnoAsync(id, usuarioId);

        if (producto is null)
        {
            return false;
        }

        _context.Productos.Remove(producto);

        await _context.SaveChangesAsync();

        return true;
    }
}