using Rubia_Divina.DTOs;

namespace Rubia_Divina.Interfaces.Services;

public interface IDashboardService
{
    Task<object> ObtenerResumenAsync();

    Task<List<ProductoMasVendidoDiaDTO>> ObtenerProdcutoMasVendidoPorDia();
}