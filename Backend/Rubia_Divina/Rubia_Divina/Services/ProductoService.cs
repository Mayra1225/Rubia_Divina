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
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerUnoAsync(int id, int usuarioId)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);
    }

    public async Task<Producto> CrearAsync(ProductoDTO dto, int usuarioId)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre.Trim(),
            Categoria = dto.Categoria.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock,
            UsuarioId = usuarioId
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task<Producto?> ActualizarAsync(int id, ProductoDTO dto, int usuarioId)
    {
        var producto = await ObtenerUnoAsync(id, usuarioId);
        if (producto is null) return null;

        producto.Nombre = dto.Nombre.Trim();
        producto.Categoria = dto.Categoria.Trim();
        producto.Descripcion = dto.Descripcion.Trim();
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task<bool> EliminarAsync(int id, int usuarioId)
    {
        var producto = await ObtenerUnoAsync(id, usuarioId);
        if (producto is null) return false;

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return true;
    }
}