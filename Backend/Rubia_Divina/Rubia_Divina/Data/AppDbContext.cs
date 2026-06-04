using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Models;

namespace Rubia_Divina.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<Promocion> Promociones => Set<Promocion>();

    public DbSet<Pedido> Pedidos => Set<Pedido>();

    public DbSet<DetallePedido> DetallePedidos => Set<DetallePedido>();

    public DbSet<HorarioPico> HorariosPico => Set<HorarioPico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Producto>()
            .Property(x => x.Precio)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Promocion>()
            .Property(x => x.Descuento)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Pedido>()
            .Property(x => x.Total)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetallePedido>()
            .Property(x => x.PrecioUnitario)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetallePedido>()
            .Property(x => x.Subtotal)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Usuario>()
            .HasMany(x => x.Productos)
            .WithOne(x => x.Usuario)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Categoria>()
            .HasMany(x => x.Productos)
            .WithOne(x => x.Categoria)
            .HasForeignKey(x => x.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pedido>()
            .HasMany(x => x.Detalles)
            .WithOne(x => x.Pedido)
            .HasForeignKey(x => x.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Producto>()
            .HasMany(x => x.Detalles)
            .WithOne(x => x.Producto)
            .HasForeignKey(x => x.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Promocion>()
            .HasMany(x => x.Pedidos)
            .WithOne(x => x.Promocion)
            .HasForeignKey(x => x.PromocionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}