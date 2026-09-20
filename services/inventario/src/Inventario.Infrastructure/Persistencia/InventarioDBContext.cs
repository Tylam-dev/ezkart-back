using Inventario.Infrastructure.Persistencia.Entidades;
using Microsoft.EntityFrameworkCore;

public class InventarioDBContext : DbContext
{
    public DbSet<Producto> Producto { get; set; } = null!;
    public InventarioDBContext(DbContextOptions<InventarioDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(InventarioDBContext).Assembly);
    }
}