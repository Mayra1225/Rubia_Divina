using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.Interfaces.Repositories;
using Rubia_Divina.Models;

namespace Rubia_Divina.Repositories;

public class HorarioPicoRepository : IHorarioPicoRepository
{
    private readonly AppDbContext _context;

    public HorarioPicoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HorarioPico>> ObtenerAsync()
    {
        return await _context.HorariosPico
            .OrderByDescending(x => x.CantidadPedidos)
            .ToListAsync();
    }

    public async Task<HorarioPico?> ObtenerPorDiaHoraAsync(
        string dia,
        int hora)
    {
        return await _context.HorariosPico
            .FirstOrDefaultAsync(x =>
                x.DiaSemana == dia &&
                x.Hora == hora);
    }

    public async Task AgregarAsync(HorarioPico horario)
    {
        await _context.HorariosPico.AddAsync(horario);
    }

    public async Task EliminarTodosAsync()
    {
        _context.HorariosPico.RemoveRange(
            _context.HorariosPico);

        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}