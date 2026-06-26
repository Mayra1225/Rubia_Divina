using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Data;
using Rubia_Divina.DTOs;
using Rubia_Divina.Interfaces.Services;
using Rubia_Divina.Models;

namespace Rubia_Divina.Services;

public class PromocionService : IPromocionService
{
    private readonly AppDbContext _context;

    public PromocionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Promocion>> ObtenerAsync()
        => await _context.Promociones.ToListAsync();

    public async Task<Promocion?> ObtenerUnoAsync(int id)
        => await _context.Promociones.FindAsync(id);

    public async Task<Promocion> CrearAsync(PromocionDTO dto)
    {
        var promo = new Promocion
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Descuento = dto.Descuento,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            Activa = dto.Activa
        };

        _context.Promociones.Add(promo);
        await _context.SaveChangesAsync();

        return promo;
    }

    public async Task<Promocion?> ActualizarAsync(int id, PromocionDTO dto)
    {
        var promo = await _context.Promociones.FindAsync(id);

        if (promo == null) return null;

        promo.Nombre = dto.Nombre;
        promo.Descripcion = dto.Descripcion;
        promo.Descuento = dto.Descuento;
        promo.FechaInicio = dto.FechaInicio;
        promo.FechaFin = dto.FechaFin;
        promo.Activa = dto.Activa;

        await _context.SaveChangesAsync();
        return promo;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var promo = await _context.Promociones.FindAsync(id);

        if (promo == null) return false;

        _context.Promociones.Remove(promo);
        await _context.SaveChangesAsync();

        return true;
    }
}