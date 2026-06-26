using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;
using Rubia_Divina.Interfaces.Services;

namespace Rubia_Divina.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> ObtenerResumenAsync()
    {
        var totalVentas = (await _context.Pedidos.ToListAsync())
            .Sum(p => p.Total);

        var totalPedidos = await _context.Pedidos.CountAsync();

        var productoMasVendido =
            await _context.DetallePedidos
                .Include(x => x.Producto)
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

        return new
        {
            totalVentas,
            totalPedidos,
            productoMasVendido,
            horarioPico
        };
    }

    public async Task<List<ProductoMasVendidoDiaDTO>> ObtenerProdcutoMasVendidoPorDia()
    {
        var datos = await _context.DetallePedidos
            .Include(x => x.Producto)
            .Include(x => x.Pedido)
            .ToListAsync();

        var resultado = datos
            .GroupBy(x => new
            {
                Dia = x.Pedido.FechaPedido.DayOfWeek,
                Producto = x.Producto.Nombre
            })
            .Select(g => new ProductoMasVendidoDiaDTO
            {
                DiaSemana = g.Key.Dia.ToString(),
                Producto = g.Key.Producto,
                CantidadVendida = g.Sum(x => x.Cantidad),
                Recomendacion = $"Análisis de {g.Key.Producto}"
            })
            .ToList();

        return resultado;
    }
}