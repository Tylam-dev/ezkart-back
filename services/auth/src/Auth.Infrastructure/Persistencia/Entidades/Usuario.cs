using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistencia.Entidades;

[Table("usuario")]
public class Usuario : EntidadBase
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Required]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;
    [Column("correo_electronico")]
    public string CorreoElectronico { get; set; } = null!;
    [Column("nombre_usuario")]
    public string NombreUsuario { get; set; } = null!;
    [Column("contrasenia")]
    public string Contrasenia { get; set; } = null!;
    [Column("rol_id")]
    public int RolId { get; set; }
    public virtual Rol Rol { get; set; } = null!;
    public virtual ICollection<RefreshToken> RefreshToken{ get; set; } = null!;
    public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasOne(x => x.Rol)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.RolId);

            builder.HasMany(x => x.RefreshToken)
                .WithOne(x => x.Usuario)
                .HasForeignKey(x => x.UsuarioId);
        }
    }
}