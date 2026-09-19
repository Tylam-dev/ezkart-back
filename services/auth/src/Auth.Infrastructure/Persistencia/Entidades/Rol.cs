using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistencia.Entidades;

public class Rol : EntidadBase
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; } = null!;
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasOne(x => x.Rol)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.RolId);
        }
    }
}