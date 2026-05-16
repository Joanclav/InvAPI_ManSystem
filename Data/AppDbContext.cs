using InventarioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Producto
        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("Producto");
            e.HasKey(p => p.Id);
            e.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            e.Property(p => p.Descripcion).HasMaxLength(255);
            e.Property(p => p.Stock).HasDefaultValue(0);
            e.Property(p => p.Precio).HasColumnType("decimal(18,2)");
            e.Property(p => p.FechaCreacion).HasDefaultValueSql("GETDATE()");
        });

        // Usuario
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("Usuario");
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.NombreUsuario).IsUnique();
            e.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(50);
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.Rol).HasMaxLength(20).HasDefaultValue("User");
        });
    }
}
