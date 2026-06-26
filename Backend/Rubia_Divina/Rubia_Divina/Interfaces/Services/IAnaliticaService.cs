using Rubia_Divina.DTOs;

namespace Rubia_Divina.Interfaces.Services;

public interface IAnaliticaService
{
    Task<AnaliticaDTO> ObtenerAnalisisAsync();
}