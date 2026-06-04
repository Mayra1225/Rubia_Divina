using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;

namespace Rubia_Divina.Services;

public class DashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> ObtenerResumenAsync()
    {
        var totalVentas = (await _context.Pedidos
    .ToListAsync())
    .Sum(p => p.Total);

        var totalPedidos =
            await _context.Pedidos.CountAsync();

        var productoMasVendido =
            await _context.DetallePedidos
                .GroupBy(x => x.Producto.Nombre)
                .Select(x => new
                {
                    Producto = x.Key,
                    Cantidad = x.Sum(d => d.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .FirstOrDefaultAsync();

        var horarioPico =
            await _context.HorariosPico
                .OrderByDescending(x => x.CantidadPedidos)
                .FirstOrDefaultAsync();

        var promoMasUsada =
            await _context.Pedidos
                .Where(x => x.PromocionId != null)
                .GroupBy(x => x.Promocion!.Nombre)
                .Select(x => new
                {
                    Promocion = x.Key,
                    Veces = x.Count()
                })
                .OrderByDescending(x => x.Veces)
                .FirstOrDefaultAsync();

        return new
        {
            totalVentas,
            totalPedidos,
            productoMasVendido,
            horarioPico,
            promoMasUsada
        };
    }
}