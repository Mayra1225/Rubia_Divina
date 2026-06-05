using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;

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

            .Select(g => new
            {
                Dia = g.Key.Dia,
                Producto = g.Key.Producto,
                Cantidad = g.Sum(x => x.Cantidad)
            })

            .ToList();

        var respuesta = resultado
            .GroupBy(x => x.Dia)
            .Select(g =>
            {
                var top =
                g.OrderByDescending(x => x.Cantidad)
                        .First();

                return new ProductoMasVendidoDiaDTO
                {
                    DiaSemana = TraducirDia(
                        top.Dia.ToString()
                        ),

                    Producto = top.Producto,

                    CantidadVendida = top.Cantidad,

                    Recomendacion = $"Crear promoción para {top.Producto} los {TraducirDia(top.Dia.ToString())}"
                };
            })

            .OrderBy(x => ObtenerOrdenDia(
                x.DiaSemana))
            .ToList();

        return respuesta;
    }

    private string TraducirDia(string dia)
    {
        return dia switch
        {
            "Monday" => "Lunes",
            "Tuesday" => "Martes",
            "Wednesday" => "Miércoles",
            "Thursday" => "Jueves",
            "Friday" => "Viernes",
            "Saturday" => "Sábado",
            "Sunday" => "Domingo",
            _ => dia
        };
    }

    private int ObtenerOrdenDia(string dia)
    {
        return dia switch
        {
            "Lunes" => 1,
            "Martes" => 2,
            "Miércoles" => 3,
            "Jueves" => 4,
            "Viernes" => 5,
            "Sábado" => 6,
            "Domingo" => 7,
            _ => 0
        };
    }
}