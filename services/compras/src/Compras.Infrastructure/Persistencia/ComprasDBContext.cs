using Compras.Infrastructure.Persistencia.Entidades;
using Microsoft.EntityFrameworkCore;

public class ComprasDBContext : DbContext
{
    public DbSet<Carrito> Carrito { get; set; } = null!;
    public DbSet<CarritoItem> CarritoItem { get; set; } = null!;
    public DbSet<Orden> Orden { get; set; } = null!;
    public DbSet<OrdenDetalle> OrdenDetalle { get; set; } = null!;
    public DbSet<DescuentoTemporada> DescuentoTemporada { get; set; } = null!;
    public ComprasDBContext(DbContextOptions<ComprasDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ComprasDBContext).Assembly);
    }
}
