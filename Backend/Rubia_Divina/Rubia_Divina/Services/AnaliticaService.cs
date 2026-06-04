using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;

namespace Rubia_Divina.Services;

public class AnaliticaService
{
    private readonly AppDbContext _context;

    public AnaliticaService(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<AnaliticaDTO> ObtenerAnalisisAsync()
    {
        var pedidos =
            await _context.Pedidos
                .ToListAsync();

        if (!pedidos.Any())
        {
            return new AnaliticaDTO
            {
                Recomendacion =
                    "No existen pedidos para analizar."
            };
        }

        var totalPedidos =
            pedidos.Count;

        var ventasTotales =
            pedidos.Sum(x => x.Total);

        var diaMasVendido =
            pedidos
            .GroupBy(x =>
                x.FechaPedido.DayOfWeek)
            .OrderByDescending(x => x.Count())
            .First()
            .Key
            .ToString();

        var horaMasVendida =
            pedidos
            .GroupBy(x =>
                x.FechaPedido.Hour)
            .OrderByDescending(x => x.Count())
            .First()
            .Key
            .ToString();

        string recomendacion;

        if (diaMasVendido == "Monday")
        {
            recomendacion =
                "Lunes presenta poca demanda. Considere promociones especiales.";
        }
        else
        {
            recomendacion =
                $"Mayor tráfico detectado en {diaMasVendido}. Se recomienda reforzar personal.";
        }

        return new AnaliticaDTO
        {
            TotalPedidos = totalPedidos,
            VentasTotales = ventasTotales,
            DiaMasVendido = diaMasVendido,
            HoraMasVendida = horaMasVendida,
            Recomendacion = recomendacion
        };
    }
}