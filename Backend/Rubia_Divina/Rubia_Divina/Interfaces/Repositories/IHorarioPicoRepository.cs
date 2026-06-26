using Rubia_Divina.Models;

namespace Rubia_Divina.Interfaces.Repositories;

public interface IHorarioPicoRepository
{
    Task<List<HorarioPico>> ObtenerAsync();

    Task<HorarioPico?> ObtenerPorDiaHoraAsync(
        string dia,
        int hora);

    Task AgregarAsync(HorarioPico horario);

    Task EliminarTodosAsync();

    Task GuardarCambiosAsync();
}