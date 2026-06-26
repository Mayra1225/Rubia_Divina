using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;
using Rubia_Divina.Interfaces.Services;

namespace Rubia_Divina.Services;

public class AnaliticaService : IAnaliticaService
{
    private readonly AppDbContext _context;

    public AnaliticaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AnaliticaDTO> ObtenerAnalisisAsync()
    {
        var pedidos = await _context.Pedidos.ToListAsync();

        if (!pedidos.Any())
        {
            return new AnaliticaDTO
            {
                Recomendacion = "No existen pedidos."
            };
        }

        return new AnaliticaDTO
        {
            TotalPedidos = pedidos.Count,
            VentasTotales = pedidos.Sum(x => x.Total),
            DiaMasVendido = pedidos
                .GroupBy(x => x.FechaPedido.DayOfWeek)
                .OrderByDescending(x => x.Count())
                .First().Key.ToString(),
            HoraMasVendida = pedidos
                .GroupBy(x => x.FechaPedido.Hour)
                .OrderByDescending(x => x.Count())
                .First().Key.ToString(),
            Recomendacion = "Análisis generado correctamente"
        };
    }
}