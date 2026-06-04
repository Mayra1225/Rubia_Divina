using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;
using Rubia_Divina.Models;

namespace Rubia_Divina.Services;

public class HorarioPicoService
{
    private readonly AppDbContext _context;

    public HorarioPicoService(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HorarioPicoDTO>>
        GenerarHorariosPicoAsync()
    {
        var pedidos =
            await _context.Pedidos
            .ToListAsync();

        if (!pedidos.Any())
        {
            return new List<HorarioPicoDTO>();
        }

        var resultado =
            pedidos
            .GroupBy(x => new
            {
                Dia =
                x.FechaPedido.DayOfWeek,

                Hora =
                x.FechaPedido.Hour
            })
            .Select(x => new HorarioPicoDTO
            {
                DiaSemana =
                    x.Key.Dia.ToString(),

                Hora =
                    x.Key.Hora,

                CantidadPedidos =
                    x.Count(),

                EsHorarioPico =
                    x.Count() >= 3
            })
            .OrderByDescending(x =>
                x.CantidadPedidos)
            .ToList();

        return resultado;
    }

    public async Task ActualizarTablaAsync()
    {
        _context.HorariosPico.RemoveRange(
            _context.HorariosPico
        );

        await _context.SaveChangesAsync();

        var pedidos =
            await _context.Pedidos
            .ToListAsync();

        var horarios =
            pedidos
            .GroupBy(x => new
            {
                Dia =
                x.FechaPedido.DayOfWeek,

                Hora =
                x.FechaPedido.Hour
            })
            .Select(x => new HorarioPico
            {
                DiaSemana =
                    x.Key.Dia.ToString(),

                Hora =
                    x.Key.Hora,

                CantidadPedidos =
                    x.Count(),

                EsHorarioPico =
                    x.Count() >= 3
            })
            .ToList();

        await _context.HorariosPico.AddRangeAsync(
            horarios
        );

        await _context.SaveChangesAsync();
    }

    public async Task<List<HorarioPico>>
        ObtenerAsync()
    {
        return await _context.HorariosPico
            .OrderByDescending(x =>
                x.CantidadPedidos)
            .ToListAsync();
    }
}