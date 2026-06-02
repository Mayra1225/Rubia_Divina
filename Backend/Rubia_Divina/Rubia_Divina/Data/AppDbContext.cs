using Microsoft.EntityFrameworkCore;
using Rubia_Divina.Models;

namespace Rubia_Divina.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options
    ) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Producto> Productos => Set<Producto>();

    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Productos)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(p => p.CategoriaId);

        modelBuilder.Entity<Categoria>()
            .HasData(
                new Categoria
                {
                    Id = 1,
                    Nombre = "Bebidas"
                },
                new Categoria
                {
                    Id = 2,
                    Nombre = "Postres"
                },
                new Categoria
                {
                    Id = 3,
                    Nombre = "Snacks"
                }
            );
    }
}