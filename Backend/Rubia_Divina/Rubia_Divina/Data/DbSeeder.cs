using Rubia_Divina.Helpers;
using Rubia_Divina.Models;

namespace Rubia_Divina.Data;

public static class DbSeeder
{
    public static void Seed(
        AppDbContext context,
        IServiceProvider services)
    {
        var passwordHelper =
            services.GetRequiredService<PasswordHelper>();

        Usuario admin;

        // USUARIO ADMIN
        if (!context.Usuarios.Any())
        {
            admin = new Usuario
            {
                Nombre = "Administrador",
                Email = "admin@rubiadivina.com",
                PasswordHash =
                    passwordHelper.HashPassword("Admin123*")
            };

            context.Usuarios.Add(admin);
            context.SaveChanges();
        }
        else
        {
            admin = context.Usuarios.First();
        }

        // CATEGORÍAS
        if (!context.Categorias.Any())
        {
            context.Categorias.AddRange(

                new Categoria
                {
                    Nombre = "Bebidas"
                },

                new Categoria
                {
                    Nombre = "Postres"
                },

                new Categoria
                {
                    Nombre = "Snacks"
                }

            );

            context.SaveChanges();
        }

        var bebidas =
            context.Categorias
                .First(c => c.Nombre == "Bebidas");

        var postres =
            context.Categorias
                .First(c => c.Nombre == "Postres");

        var snacks =
            context.Categorias
                .First(c => c.Nombre == "Snacks");

        // PRODUCTOS
        if (!context.Productos.Any())
        {
            context.Productos.AddRange(

                new Producto
                {
                    Nombre = "Capuccino",
                    Descripcion = "Café espresso con leche cremosa",
                    Precio = 3.50m,
                    Stock = 20,
                    UsuarioId = admin.Id,
                    CategoriaId = bebidas.Id,
                    ImagenUrl = "https://images.pexels.com/photos/312418/pexels-photo-312418.jpeg"
                },

                new Producto
                {
                    Nombre = "Latte",
                    Descripcion = "Café latte tradicional",
                    Precio = 4.00m,
                    Stock = 25,
                    UsuarioId = admin.Id,
                    CategoriaId = bebidas.Id,
                    ImagenUrl = "https://images.pexels.com/photos/302899/pexels-photo-302899.jpeg"
                },

                new Producto
                {
                    Nombre = "Cheesecake",
                    Descripcion = "Cheesecake artesanal",
                    Precio = 4.25m,
                    Stock = 15,
                    UsuarioId = admin.Id,
                    CategoriaId = postres.Id,
                    ImagenUrl = "https://images.pexels.com/photos/291528/pexels-photo-291528.jpeg"
                },

                new Producto
                {
                    Nombre = "Brownie",
                    Descripcion = "Brownie de chocolate",
                    Precio = 3.00m,
                    Stock = 30,
                    UsuarioId = admin.Id,
                    CategoriaId = postres.Id,
                    ImagenUrl = "https://images.pexels.com/photos/45202/brownie-chocolate-dessert-cake-45202.jpeg"
                }

            );

            context.SaveChanges();
        }

        // PROMOCIONES
        if (!context.Promociones.Any())
        {
            context.Promociones.AddRange(

                new Promocion
                {
                    Nombre = "3x2 Capuccino",
                    Descripcion = "Compra 3 y paga 2",
                    Descuento = 33,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddMonths(1),
                    Activa = true
                },

                new Promocion
                {
                    Nombre = "Postre Gratis",
                    Descripcion = "Postre gratis por compras mayores a $10",
                    Descuento = 15,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddMonths(1),
                    Activa = true
                }

            );

            context.SaveChanges();
        }

        // HORARIOS PICO
        if (!context.HorariosPico.Any())
        {
            context.HorariosPico.AddRange(

            /*new HorarioPico
            {
                DiaSemana = "Lunes",
                HoraInicio = new TimeSpan(8, 0, 0),
                HoraFin = new TimeSpan(10, 0, 0),
                CantidadPedidos = 30
            },

            new HorarioPico
            {
                DiaSemana = "Viernes",
                HoraInicio = new TimeSpan(17, 0, 0),
                HoraFin = new TimeSpan(19, 0, 0),
                CantidadPedidos = 85
            }*/

            );

            context.SaveChanges();
        }

        if (!context.Pedidos.Any())
        {
            context.Pedidos.AddRange(

                new Pedido
                {
                    FechaPedido =
                        DateTime.Now.AddDays(-2).AddHours(9),

                    Total = 15,

                    Detalles = new List<DetallePedido>
                    {
                new DetallePedido
                {
                    ProductoId = 1,
                    Cantidad = 2,
                    PrecioUnitario = 3.50m,
                    Subtotal = 7.00m
                },
                new DetallePedido
                {
                    ProductoId = 4,
                    Cantidad = 2,
                    PrecioUnitario = 4.00m,
                    Subtotal = 8.00m
                }
                    }
                },

                new Pedido
                {
                    FechaPedido =
                        DateTime.Now.AddDays(-2).AddHours(9),

                    Total = 20,

                    Detalles = new List<DetallePedido>
                    {
                new DetallePedido
                {
                    ProductoId = 2,
                    Cantidad = 5,
                    PrecioUnitario = 4.00m,
                    Subtotal = 20.00m
                }
                    }
                },

                new Pedido
                {
                    FechaPedido =
                        DateTime.Now.AddDays(-2).AddHours(9),

                    Total = 10,

                    Detalles = new List<DetallePedido>
                    {
                new DetallePedido
                {
                    ProductoId = 3,
                    Cantidad = 2,
                    PrecioUnitario = 5.00m,
                    Subtotal = 10.00m
                }
                    }
                },

                new Pedido
                {
                    FechaPedido =
                        DateTime.Now.AddDays(-1).AddHours(18),

                    PromocionId = 1,

                    Total = 30,

                    Detalles = new List<DetallePedido>
                    {
                new DetallePedido
                {
                    ProductoId = 1,
                    Cantidad = 4,
                    PrecioUnitario = 3.50m,
                    Subtotal = 14.00m
                },
                new DetallePedido
                {
                    ProductoId = 2,
                    Cantidad = 4,
                    PrecioUnitario = 4.00m,
                    Subtotal = 16.00m
                }
                    }
                },

                new Pedido
                {
                    FechaPedido =
                        DateTime.Now.AddDays(-1).AddHours(18),

                    PromocionId = 2,

                    Total = 25,

                    Detalles = new List<DetallePedido>
                    {
                new DetallePedido
                {
                    ProductoId = 3,
                    Cantidad = 3,
                    PrecioUnitario = 4.25m,
                    Subtotal = 12.75m
                },
                new DetallePedido
                {
                    ProductoId = 4,
                    Cantidad = 4,
                    PrecioUnitario = 3.00m,
                    Subtotal = 12.00m
                }
                    }
                }
            );

            context.SaveChanges();
        }


        context.SaveChanges();
    }
}