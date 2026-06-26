using Rubia_Divina.Models;

namespace Rubia_Divina.Interfaces;

public interface IPromocionFactory
{
    Promocion CrearPromocion(
        string nombre,
        string descripcion,
        decimal descuento);
}