using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.Interfaces.Repositories;
using Rubia_Divina.Models;

namespace Rubia_Divina.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
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
            .FirstOrDefaultAsync(p =>
                p.Id == id &&
                p.UsuarioId == usuarioId);
    }

    public async Task<Categoria?> ObtenerCategoriaAsync(int categoriaId)
    {
        return await _context.Categorias
            .FindAsync(categoriaId);
    }

    public async Task AgregarAsync(Producto producto)
    {
        await _context.Productos.AddAsync(producto);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task EliminarAsync(Producto producto)
    {
        _context.Productos.Remove(producto);

        return Task.CompletedTask;
    }

    public async Task<List<Producto>> ObtenerPorCategoriaAsync(
    int categoriaId,
    int usuarioId)
    {
        return await _context.Productos
            .Include(p => p.Categoria)
            .Where(p =>
                p.CategoriaId == categoriaId &&
                p.UsuarioId == usuarioId)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }
}