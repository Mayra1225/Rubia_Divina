
namespace Rubia_Divina.DTOs;

public class ProductoMasVendidoDiaDTO
{
    public string DiaSemana { get; set; } = string.Empty;

    public string Producto { get; set; } = string.Empty;

    public int CantidadVendida { get; set; }

    public string Recomendacion { get; set; } = string.Empty;
}