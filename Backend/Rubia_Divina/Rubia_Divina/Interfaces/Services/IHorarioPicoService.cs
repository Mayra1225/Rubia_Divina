using Rubia_Divina.Models;
using Rubia_Divina.DTOs;

namespace Rubia_Divina.Interfaces.Services;

public interface IHorarioPicoService
{
    Task<List<HorarioPicoDTO>> GenerarHorariosPicoAsync();

    Task ActualizarTablaAsync();

    Task<List<HorarioPico>> ObtenerAsync();
}