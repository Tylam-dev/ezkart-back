using Auth.Domain;
using Microsoft.EntityFrameworkCore;

public class AuthDBContext : DbContext
{
    public DbSet<Usuario> Usuario { get; set; } = null!;
    public DbSet<Rol> Rol { get; set; } = null!;
    public AuthDBContext(DbContextOptions<AuthDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AuthDBContext).Assembly);
    }
}