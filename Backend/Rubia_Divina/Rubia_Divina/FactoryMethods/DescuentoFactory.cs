using Rubia_Divina.Interfaces;
using Rubia_Divina.Models;

namespace Rubia_Divina.FactoryMethods;

public class DescuentoFactory : IPromocionFactory
{
    public Promocion CrearPromocion(
        string nombre,
        string descripcion,
        decimal descuento)
    {
        return new Promocion
        {
            Nombre = nombre,
            Descripcion = descripcion,
            Descuento = descuento,
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddDays(15),
            Activa = true
        };
    }
}