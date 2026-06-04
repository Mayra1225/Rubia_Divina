namespace Rubia_Divina.DTOs;

public class HorarioPicoDTO
{
    public string DiaSemana { get; set; }
        = string.Empty;

    public int Hora { get; set; }

    public int CantidadPedidos { get; set; }

    public bool EsHorarioPico { get; set; }
}