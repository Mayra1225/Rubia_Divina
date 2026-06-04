namespace Rubia_Divina.DTOs;

public class AnaliticaDTO
{
    public int TotalPedidos { get; set; }

    public decimal VentasTotales { get; set; }

    public string DiaMasVendido { get; set; }
        = string.Empty;

    public string HoraMasVendida { get; set; }
        = string.Empty;

    public string Recomendacion
    {
        get; set;
    } = string.Empty;
}