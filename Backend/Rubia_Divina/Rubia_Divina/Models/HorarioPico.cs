namespace Rubia_Divina.Models;

public class HorarioPico
{
    public int Id { get; set; }

    public string DiaSemana { get; set; }
        = string.Empty;

    public int Hora { get; set; }

    public int CantidadPedidos { get; set; }

    public bool EsHorarioPico { get; set; }
}