using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.Interfaces.Repositories;
using Rubia_Divina.Models;

namespace Rubia_Divina.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pedido>> ObtenerTodosAsync()
    {
        return await _context.Pedidos
            .Include(x => x.Detalles)
            .ThenInclude(x => x.Producto)
            .Include(x => x.Promocion)
            .OrderByDescending(x => x.FechaPedido)
            .ToListAsync();
    }

    public async Task<Pedido?> ObtenerUnoAsync(int id)
    {
        return await _context.Pedidos
            .Include(x => x.Detalles)
            .ThenInclude(x => x.Producto)
            .Include(x => x.Promocion)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Producto?> ObtenerProductoAsync(int productoId)
    {
        return await _context.Productos
            .FirstOrDefaultAsync(x => x.Id == productoId);
    }

    public async Task<Promocion?> ObtenerPromocionAsync(int promocionId)
    {
        return await _context.Promociones
            .FirstOrDefaultAsync(x =>
                x.Id == promocionId &&
                x.Activa);
    }

    public async Task AgregarPedidoAsync(Pedido pedido)
    {
        await _context.Pedidos.AddAsync(pedido);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task EliminarPedidoAsync(Pedido pedido)
    {
        _context.Pedidos.Remove(pedido);

        return Task.CompletedTask;
    }

    public Task EliminarDetallesAsync(IEnumerable<DetallePedido> detalles)
    {
        _context.DetallePedidos.RemoveRange(detalles);

        return Task.CompletedTask;
    }
}