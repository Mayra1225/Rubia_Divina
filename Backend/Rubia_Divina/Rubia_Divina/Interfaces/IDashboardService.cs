using Rubia_Divina.DTOs;

namespace Rubia_Divina.Interfaces;

public interface IDashboardService
{
    Task<DashboardDTO> ObtenerResumenAsync();
}