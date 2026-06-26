using Rubia_Divina.FactoryMethods;
using Rubia_Divina.Interfaces;

namespace Rubia_Divina.Factories;

public static class PromocionFactory
{
    public static IPromocionFactory ObtenerFactory(
        string tipo)
    {
        return tipo switch
        {
            "Combo" => new ComboFactory(),

            "Descuento" => new DescuentoFactory(),

            "Temporada" => new TemporadaFactory(),

            _ => throw new Exception(
                "Tipo de promoción inválido")
        };
    }
}