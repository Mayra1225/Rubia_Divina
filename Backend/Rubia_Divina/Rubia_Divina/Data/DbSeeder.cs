using Rubia_Divina.Helpers;
using Rubia_Divina.Models;

namespace Rubia_Divina.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context, IServiceProvider serviceProvider)
    {
        if (context.Usuarios.Any())
        {
            return;
        }

        var passwordHelper = serviceProvider.GetRequiredService<PasswordHelper>();

        var admin = new Usuario
        {
            Email = "admin@rubiadivina.com",
            PasswordHash = passwordHelper.HashPassword("Admin123*"),
            Nombre = "Administrador"
        };

        context.Usuarios.Add(admin);
        context.SaveChanges();

        context.Productos.AddRange(
    new Producto
    {
        Nombre = "Capuccino",
        Descripcion = "Café espresso con leche cremosa.",
        Precio = 3.50m,
        Stock = 15,
        UsuarioId = admin.Id,
        CategoriaId = 1
    },
    new Producto
    {
        Nombre = "Cheesecake",
        Descripcion = "Porción individual de cheesecake artesanal.",
        Precio = 4.25m,
        Stock = 8,
        UsuarioId = admin.Id,
        CategoriaId = 2
    }
);

        context.SaveChanges();
    }
}