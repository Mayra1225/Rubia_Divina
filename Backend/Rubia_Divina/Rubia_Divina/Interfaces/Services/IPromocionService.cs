using Rubia_Divina.DTOs;
using Rubia_Divina.Models;

namespace Rubia_Divina.Interfaces.Services;

public interface IPromocionService
{
    Task<List<Promocion>> ObtenerAsync();

    Task<Promocion?> ObtenerUnoAsync(int id);

    Task<Promocion> CrearAsync(PromocionDTO dto);

    Task<Promocion?> ActualizarAsync(
        int id,
        PromocionDTO dto);

    Task<bool> EliminarAsync(int id);
}