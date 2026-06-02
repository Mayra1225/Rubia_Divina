using Rubia_Divina.Helpers;
using Rubia_Divina.Models;

namespace Rubia_Divina.Data;

public static class DbSeeder
{
    public static void Seed(
        AppDbContext context,
        IServiceProvider serviceProvider
    )
    {
        if (context.Usuarios.Any())
        {
            return;
        }

        var passwordHelper =
            serviceProvider.GetRequiredService<PasswordHelper>();

        var admin = new Usuario
        {
            Email = "admin@rubiadivina.com",
            PasswordHash =
                passwordHelper.HashPassword(
                    "Admin123*"
                ),
            Nombre = "Administrador"
        };

        context.Usuarios.Add(admin);

        context.SaveChanges();

        context.Productos.AddRange(
            new Producto
            {
                Nombre = "Capuccino",
                Descripcion =
                    "Café espresso con leche cremosa.",
                Precio = 3.50m,
                Stock = 15,
                UsuarioId = admin.Id,
                CategoriaId = 1,
                ImagenUrl =
                    "https://images.unsplash.com/photo-1511920170033-f8396924c348"
            },
            new Producto
            {
                Nombre = "Cheesecake",
                Descripcion =
                    "Cheesecake artesanal.",
                Precio = 4.25m,
                Stock = 8,
                UsuarioId = admin.Id,
                CategoriaId = 2,
                ImagenUrl =
                    "https://images.unsplash.com/photo-1565958011703-44f9829ba187"
            },
            new Producto
            {
                Nombre = "Latte Vainilla",
                Descripcion =
                    "Latte con esencia de vainilla.",
                Precio = 5.00m,
                Stock = 10,
                UsuarioId = admin.Id,
                CategoriaId = 1,
                ImagenUrl =
                    "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085"
            },
            new Producto
            {
                Nombre = "Brownie Chocolate",
                Descripcion =
                    "Brownie artesanal de chocolate.",
                Precio = 3.75m,
                Stock = 12,
                UsuarioId = admin.Id,
                CategoriaId = 2,
                ImagenUrl =
                    "https://images.unsplash.com/photo-1606313564200-e75d5e30476c"
            }
        );

        context.SaveChanges();
    }
}